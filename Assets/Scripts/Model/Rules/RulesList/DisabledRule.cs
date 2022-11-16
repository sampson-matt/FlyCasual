using UnityEngine;
using Ship;
using Tokens;
using Movement;
using System.Linq;
using Editions;
using ActionsList;
using System;

namespace RulesList
{
    public class DisabledRule
    {
        static bool RuleIsInitialized = false;

        public DisabledRule()
        {
            if (!RuleIsInitialized)
            {
                GenericShip.OnNoManeuverWasRevealedGlobal += SetDisabledManeuver;
                RuleIsInitialized = true;
            }
        }

        public static void SetDisabledManeuver(GenericShip ship)
        {
            if (ship.State.IsDisabled)
            {
                AssignDisabledManeuver(ship);
                ship.OnTryAddAction += DisabledShipCanNotAct;
            }
        }

        private static void DisabledShipCanNotAct(GenericShip ship, GenericAction action, ref bool canBePerformed)
        {
            canBePerformed = false;
        }

        private static void AssignDisabledManeuver(GenericShip ship)
        {
            GenericMovement disabledMovement = new StationaryMovement(
                0,
                ManeuverDirection.Stationary,
                ManeuverBearing.Stationary,
                ship.GetColorComplexityOfManeuver(
                    new ManeuverHolder(
                        ManeuverSpeed.Speed0,
                        ManeuverDirection.Stationary,
                        ManeuverBearing.Stationary,
                        MovementComplexity.Normal
                    )
                )
            ) {
                IsRevealDial = false, IsIonManeuver = false
            };
            ship.SetAssignedManeuver(disabledMovement);
        }
    }
}
