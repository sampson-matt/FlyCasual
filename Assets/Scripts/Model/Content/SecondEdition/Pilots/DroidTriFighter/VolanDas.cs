using BoardTools;
using Ship;
using SubPhases;
using System.Linq;
using Movement;
using Upgrade;
using System.Collections.Generic;

namespace Ship.SecondEdition.DroidTriFighter
{
    public class VolanDas : DroidTriFighter
    {
        public VolanDas()
        {
            PilotInfo = new PilotCardInfo(
                "Volan Das",
                5,
                40,
                true,
                extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Illicit },
                abilityType: typeof(Abilities.SecondEdition.VolanDasAbility),
                affectedByStandardized: false
            );
            ShipInfo.ActionIcons.SwitchToOrganicActions();
            DialInfo.ChangeManeuverComplexity(new ManeuverHolder(ManeuverSpeed.Speed2, ManeuverDirection.Left, ManeuverBearing.Bank), MovementComplexity.Easy);
            DialInfo.ChangeManeuverComplexity(new ManeuverHolder(ManeuverSpeed.Speed2, ManeuverDirection.Right, ManeuverBearing.Bank), MovementComplexity.Easy);
            DialInfo.ChangeManeuverComplexity(new ManeuverHolder(ManeuverSpeed.Speed3, ManeuverDirection.Left, ManeuverBearing.Bank), MovementComplexity.Easy);
            DialInfo.ChangeManeuverComplexity(new ManeuverHolder(ManeuverSpeed.Speed3, ManeuverDirection.Right, ManeuverBearing.Bank), MovementComplexity.Easy);
            DialInfo.ChangeManeuverComplexity(new ManeuverHolder(ManeuverSpeed.Speed3, ManeuverDirection.Left, ManeuverBearing.Turn), MovementComplexity.Normal);
            DialInfo.ChangeManeuverComplexity(new ManeuverHolder(ManeuverSpeed.Speed3, ManeuverDirection.Right, ManeuverBearing.Turn), MovementComplexity.Normal);
        }
    }
}

namespace Abilities.SecondEdition
{
    public class VolanDasAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully += RegisterMovementTrigger;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully -= RegisterMovementTrigger;
        }

        protected void RegisterMovementTrigger(GenericShip ship)
        {
            if (HostShip.GetLastManeuverColor() == MovementComplexity.Complex && Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(1, 1), Team.Type.Enemy).Any())
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, AskAbility);
            }
        }

        private void AskAbility(object sender, System.EventArgs e)
        {
            SelectTargetForAbility(
                    AssignStrain,
                    FilterAbilityTargets,
                    GetAiAbilityPriority,
                    HostShip.Owner.PlayerNo,
                    HostName,
                    "You may choose an enemy ship at range 1 to gain 1 strain token and you may remove 1 stress token.",
                    HostShip
                );
        }

        private int GetAiAbilityPriority(GenericShip ship)
        {
            var result = 0;

            result += ship.PilotInfo.Cost;

            ShotInfo shotInfo = new ShotInfo(HostShip, ship, HostShip.PrimaryWeapons);
            if (shotInfo.IsShotAvailable)
            {
                result *= 2;
            }

            return result;
        }

        private bool FilterAbilityTargets(GenericShip ship)
        {
            return ship.Owner != HostShip.Owner && Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(1, 1), Team.Type.Enemy).Contains(ship);
        }
        private void AssignStrain()
        {
            if (TargetShip != null)
            {
                TargetShip.Tokens.AssignToken(typeof(Tokens.StrainToken), delegate { });
                HostShip.Tokens.RemoveToken(typeof(Tokens.StressToken), SelectShipSubPhase.FinishSelection);
            }
            else
            {
                SelectShipSubPhase.FinishSelection();
            }
        }

    }
}
