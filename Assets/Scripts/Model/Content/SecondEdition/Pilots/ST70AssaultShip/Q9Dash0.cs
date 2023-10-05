using ActionsList;
using Content;
using Ship;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ST70AssaultShip
    {
        public class Q9Dash0 : ST70AssaultShip
        {
            public Q9Dash0() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Q9-0",
                    5,
                    52,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.Q9Dash0Ability),
                    extraUpgradeIcon: UpgradeType.Talent,
                    tags: new List<Tags>
                    {
                        Tags.Droid
                    }
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class Q9Dash0Ability : GenericAbility
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
            if (ship.AssignedManeuver != null && ship.AssignedManeuver.IsAdvancedManeuver)
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, AskToPerformStrainedActions);
            }
        }

        private void AskToPerformStrainedActions(object sender, EventArgs e)
        {
            HostShip.OnActionIsPerformed += CheckGainStrainToken;

            HostShip.AskPerformFreeAction
            (
                new List<GenericAction>
                {
                    new CalculateAction() {CanBePerformedWhileStressed = true},
                    new BarrelRollAction() {CanBePerformedWhileStressed = true}
                },
                Triggers.FinishTrigger,
                descriptionShort: HostShip.PilotInfo.PilotName,
                descriptionLong: "You may perform one of these actions. If you do, gain 1 strain token.",
                imageHolder: HostShip
            );
        }

        private void CheckGainStrainToken(GenericAction action)
        {
            HostShip.OnActionIsPerformed -= CheckGainStrainToken;

            if (action != null)
            {
                RegisterAbilityTrigger(TriggerTypes.OnActionIsPerformed, GainStrainToken);
            };
        }

        private void GainStrainToken(object sender, EventArgs e)
        {
            HostShip.Tokens.AssignToken(typeof(Tokens.StrainToken), Triggers.FinishTrigger);
        }
    }
}