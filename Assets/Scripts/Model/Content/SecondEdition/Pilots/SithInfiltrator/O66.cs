using System;
using System.Collections.Generic;
using Ship;
using Upgrade;

namespace Ship.SecondEdition.SithInfiltrator
{
    public class O66 : SithInfiltrator
    {
        public O66()
        {
            PilotInfo = new PilotCardInfo(
                "0-66",
                3,
                47,
                true,
                abilityType: typeof(Abilities.SecondEdition.O66PilotAbility),
                extraUpgradeIcon: UpgradeType.Talent
            );

            ShipInfo.ActionIcons.SwitchToDroidActions();
        }
    }
}

namespace Abilities.SecondEdition
{
    public class O66PilotAbility : GenericAbility
    {
        public GenericShip PreviousAttacker { get; private set; }
        public GenericShip PreviousDefender { get; private set; }

        public override void ActivateAbility()
        {
            HostShip.OnAttackFinishAsDefender += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackFinishAsDefender -= RegisterAbility;
        }

        private void RegisterAbility(GenericShip ship)
        {
            RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, UseCalculateToPerformAction);
        }

        private void UseCalculateToPerformAction(object sender, EventArgs e)
        {
            if (HostShip.Tokens.HasToken(typeof(Tokens.CalculateToken)))
            {
                PreviousAttacker = Combat.Attacker;
                PreviousDefender = Combat.Defender;

                Selection.DeselectAllShips();
                Selection.ChangeActiveShip(HostShip);

                AskToUseAbility(
                    HostShip.PilotInfo.PilotName,
                    NeverUseByDefault,
                    AgreeToPerformAction,
                    descriptionLong: "Do you want to spend a calculate token to perform an action?",
                    imageHolder: HostShip
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void AgreeToPerformAction(object sender, EventArgs e)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();
            HostShip.Tokens.SpendToken(typeof(Tokens.CalculateToken), PerformFreeAction);
        }

        private void PerformFreeAction()
        {
            HostShip.AskPerformFreeAction(
                HostShip.GetAvailableActions(),
                FinishAbility,
                HostShip.PilotInfo.PilotName,
                "After you defend, you may spend 1 Calculate token to perform an action",
                HostShip
            );
        }

        private void FinishAbility()
        {
            Selection.ChangeActiveShip(PreviousAttacker);
            Selection.ChangeAnotherShip(PreviousDefender);

            Triggers.FinishTrigger();
        }
    }
}