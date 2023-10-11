using Ship;
using BoardTools;
using SubPhases;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.YT2400LightFreighter2023
    {
        public class Leebo2023LSL : YT2400LightFreighter2023
        {
            public Leebo2023LSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Leebo",
                    3,
                    69,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.Leebo2023Ability),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Crew);

                PilotNameCanonical = "leebo-swz103-sl-rebelalliance";
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/YT2400/leebo.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class Leebo2023Ability : GenericAbility
    {

        //At the end of the Engagement Phase, you may spend a calculate token to acquire a lock on an enemy ship at range 2-3

        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseEnd_Triggers += TryRegisterLeeboAbiliity;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseEnd_Triggers -= TryRegisterLeeboAbiliity;
        }

        private void TryRegisterLeeboAbiliity()
        {
            if (HostShip.Tokens.HasToken(typeof(Tokens.CalculateToken)) && BoardTools.Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(2,3),Team.Type.Enemy).Count >0 )
            {
                RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseEnd, AskToUseLeeboAbility);
            }
        }

        private void AskToUseLeeboAbility(object sender, System.EventArgs e)
        {
            SelectTargetForAbility(
                    GrantFreeTargetLock,
                    FilterAbilityTargets,
                    GetAiAbilityPriority,
                    HostShip.Owner.PlayerNo,
                    HostName,
                    "You may spend 1 calculate token to aquire a lock on an enemy at range 2-3",
                    HostShip
                );
        }

        private int GetAiAbilityPriority(GenericShip ship)
        {
            int priority = 0;

            if (!HostShip.Tokens.HasToken(typeof(BlueTargetLockToken))) priority += 50;
            ShotInfo shotInfo = new ShotInfo(HostShip, ship, ship.PrimaryWeapons);
            if (shotInfo.IsShotAvailable) priority += 40;

            priority += ship.PilotInfo.Cost;

            return priority;
        }

        private bool FilterAbilityTargets(GenericShip ship)
        {
            var range = new DistanceInfo(HostShip, ship).Range;
            return ship.Owner != HostShip.Owner && range > 1;
        }

        private void GrantFreeTargetLock()
        {
            if (TargetShip != null)
            {
                ActionsHolder.AcquireTargetLock(HostShip, TargetShip, SelectShipSubPhase.FinishSelection, SelectShipSubPhase.FinishSelection);
                HostShip.Tokens.SpendToken(
                    typeof(Tokens.CalculateToken),
                    delegate { }
                );
            }
            else
            {
                SelectShipSubPhase.FinishSelection();
            }
        }

    }
}

