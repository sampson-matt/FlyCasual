using System.Collections.Generic;
using Upgrade;
using System;
using Abilities.SecondEdition;
using System.Linq;
using Movement;
using Ship;
using Tokens;
using SubPhases;
using Conditions;

namespace Ship
{
    namespace SecondEdition.LaatIGunship
    {
        public class Sicko : LaatIGunship
        {
            public Sicko() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Sicko\"",
                    2,
                    49,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.SickoAbility)
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class SickoAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnMovementFinish += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementFinish -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship)
        {
            if (ship.AssignedManeuver != null && ship.AssignedManeuver.IsBasicManeuver)
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, AskToAssignSickeningManeuverCondition);
            }
        }

        private void AskToAssignSickeningManeuverCondition(object sender, EventArgs e)
        {
            AskToUseAbility
             (
                 HostShip.PilotInfo.PilotName,
                 NeverUseByDefault,
                 AssignSickeningManeuverCondition,
                 descriptionLong: "Do you want to assign the Sickening Maneuver condition to yourself?",
                 imageHolder: HostShip
             );
        }

        private void AssignSickeningManeuverCondition(object sender, EventArgs e)
        {
            HostShip.Tokens.AssignCondition(new SickeningManeuver(HostShip) { SourceUpgrade = HostUpgrade });
            DecisionSubPhase.ConfirmDecision();
        }
    }
}

namespace Conditions
{
    public class SickeningManeuver : GenericToken
    {
        public GenericUpgrade SourceUpgrade;
        public SickeningManeuver(GenericShip host) : base(host)
        {
            Name = ImageName = "Sickening Maneuver Condition";
            Temporary = false;
            Tooltip = "https://infinitearenas.com/xw2/images/conditions/sickeningmaneuver.png";
        }

        public override void WhenAssigned()
        {
            Host.OnManeuverIsRevealed += CheckRevealedManeuved;
            Host.OnMovementFinish += RemoveCondition;
            Host.OnTryCanPerformRedManeuverWhileStressed += CheckRedManeuversWhileStressed;
        }

        public override void WhenRemoved()
        {
            Host.OnManeuverIsRevealed -= CheckRevealedManeuved;
            Host.OnMovementFinish -= RemoveCondition;
            Host.OnTryCanPerformRedManeuverWhileStressed -= CheckRedManeuversWhileStressed;
        }

        private void CheckRevealedManeuved(GenericShip ship)
        {
            if (ship.RevealedManeuver != null
                && (ship.RevealedManeuver.Bearing == Movement.ManeuverBearing.Bank || ship.RevealedManeuver.Bearing == Movement.ManeuverBearing.Turn))
            {
                DoSideSlip();
            } else if (ship.RevealedManeuver.Bearing == ManeuverBearing.Straight)
            {
                DoKTurn();
            }
        }

        private void DoSideSlip()
        {
            if (Host.RevealedManeuver.Bearing == ManeuverBearing.Bank)
            {
                GenericMovement movement = new SideslipBankMovement(
                    Host.RevealedManeuver.Speed,
                    Host.RevealedManeuver.Direction,
                    ManeuverBearing.SideslipBank,
                    Host.RevealedManeuver.ColorComplexity
                );

                Messages.ShowInfo("Maneuver is changed to Sideslip");
                Host.SetAssignedManeuver(movement);
            }
            else if (Host.RevealedManeuver.Bearing == ManeuverBearing.Turn)
            {
                GenericMovement movement = new SideslipTurnMovement(
                    Host.RevealedManeuver.Speed,
                    Host.RevealedManeuver.Direction,
                    ManeuverBearing.SideslipTurn,
                    Host.RevealedManeuver.ColorComplexity
                );

                Messages.ShowInfo("Maneuver is changed to Sideslip");
                Host.SetAssignedManeuver(movement);
            }
            Host.Tokens.AssignToken(typeof(Tokens.StrainToken), delegate { });
        }

        private void DoKTurn()
        {
            GenericMovement movement = new KoiogranTurnMovement(
                Host.RevealedManeuver.Speed,
                Host.RevealedManeuver.Direction,
                ManeuverBearing.KoiogranTurn,
                MovementComplexity.Complex
            );

            Messages.ShowInfo($"{Host.PilotInfo.PilotName}: Maneuver is changed to Koiogran Turn");
            Host.SetAssignedManeuver(movement);
        }

        private void CheckRedManeuversWhileStressed(ref bool isAllowed)
        {
            Messages.ShowInfo("Sickening Maneuver: Red maneuver is allowed");
            isAllowed = true;
        }

        private void RemoveCondition(GenericShip ship)
        {
            Host.Tokens.RemoveCondition(this);
        }
    }
}