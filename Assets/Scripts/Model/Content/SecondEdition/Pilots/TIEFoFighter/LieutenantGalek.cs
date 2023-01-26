using ActionsList;
using Ship;
using System;
using BoardTools;
using Upgrade;
using Tokens;
using Actions;
using System.Linq;

namespace Ship
{
    namespace SecondEdition.TIEFoFighter
    {
        public class LieutenantGalek : TIEFoFighter
        {
            public LieutenantGalek() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Lieutenant Galek",
                    5,
                    32,
                    pilotTitle: "Harsh Instructor",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.LieutenantGalekAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ImageUrl = "https://infinitearenas.com/xw2legacy/images/pilots/lieutenantgalek.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class LieutenantGalekAbility : GenericAbility
    {
        private GenericShip PreviousCurrentShip { get; set; }
        public override void ActivateAbility()
        {
            GenericShip.OnShipIsDestroyedGlobal += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnShipIsDestroyedGlobal -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship, bool flag)
        {
            if (!Tools.IsFriendly(HostShip, ship) || Tools.IsSameShip(HostShip, ship)) return;

            DistanceInfo distanceInfo = new DistanceInfo(HostShip, ship);
            if (distanceInfo.Range > 2) return;

            RegisterAbilityTrigger(
                TriggerTypes.OnShipIsDestroyed,
                AskWhatToDo,
                customTriggerName: $"{HostShip.PilotInfo.PilotName} (ID: {HostShip.ShipId})"
            );
        }

        private void AskWhatToDo(object sender, EventArgs e)
        {
            PreviousCurrentShip = Selection.ThisShip;

            Selection.ChangeActiveShip(HostShip);
            HostShip.OnCheckCoordinateModeModification += SetCustomCoordinateMode;
            Selection.ThisShip.AskPerformFreeAction(
                new CoordinateAction() { CanBePerformedWhileStressed = true},
                FinishAbility,
                descriptionShort: HostShip.PilotInfo.PilotName,
                descriptionLong: "You may perform a Coordinate action, even while stressed",
                imageHolder: HostShip
            );
        }

        private void FinishAbility()
        {
            Selection.ChangeActiveShip(PreviousCurrentShip);
            Triggers.FinishTrigger();
        }

        private void SetCustomCoordinateMode(ref CoordinateActionData coordinateActionData)
        {
            coordinateActionData.SameActionLimit = true;

            coordinateActionData.GetAiPriority = GetAiPriority;

            HostShip.OnCheckCoordinateModeModification -= SetCustomCoordinateMode;
        }

        private int GetAiPriority(GenericShip ship)
        {
            int result = 0;

            result += NeedTokenPriority(ship);
            result += ship.PilotInfo.Cost + ship.UpgradeBar.GetUpgradesOnlyFaceup().Sum(n => n.UpgradeInfo.Cost);

            return result;
        }

        private int NeedTokenPriority(GenericShip ship)
        {
            if (!ship.Tokens.HasToken(typeof(FocusToken))) return 100;
            if (ship.ActionBar.HasAction(typeof(EvadeAction)) && !ship.Tokens.HasToken(typeof(EvadeToken))) return 50;
            if (ship.ActionBar.HasAction(typeof(TargetLockAction)) && !ship.Tokens.HasToken(typeof(BlueTargetLockToken), '*')) return 50;
            return 0;
        }
    }
}