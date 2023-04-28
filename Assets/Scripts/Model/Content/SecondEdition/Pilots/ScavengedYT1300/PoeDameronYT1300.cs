using ActionsList;
using SubPhases;
using Ship;
using System;
using Upgrade;
using Actions;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.ScavengedYT1300
    {
        public class PoeDameronYT1300 : ScavengedYT1300
        {
            public PoeDameronYT1300() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Poe Dameron",
                    6,
                    65,
                    isLimited: true,
                    charges: 2,
                    regensCharges: 1,
                    abilityType: typeof(Abilities.SecondEdition.PoeDameronYT1300PilotAbility),
                    pilotTitle: "A Difficult Man",
                    extraUpgradeIcon: UpgradeType.Talent
                );

                PilotNameCanonical = "poedameron-scavengedyt1300";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class PoeDameronYT1300PilotAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnMovementActivationStart += RegisterTriggerActivationPhase;
            HostShip.OnMovementFinishSuccessfully += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementActivationStart -= RegisterTriggerActivationPhase;
            HostShip.OnMovementFinishSuccessfully -= RegisterAbility;
        }

        private void RegisterAbility(GenericShip ship)
        {
            if (HostShip.State.Charges > 1)
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, PerformFreeFocusAction);
            }       
        }

        private void PerformFreeFocusAction(object sender, System.EventArgs e)
        {
            HostShip.BeforeActionIsPerformed += RegisterSpendChargeTrigger;
            HostShip.OnActionIsPerformed += RegisterExposeDamageCardTrigger;
            HostShip.AskPerformFreeAction(
                new List<GenericAction>()
                {
                    new BoostAction(),
                    new BarrelRollAction() { Color = ActionColor.Red }
                },
                CleanUp,
                HostShip.PilotInfo.PilotName,
                "After you fully execute a maneuver you may spend 2 Charges to perform a white Boost or a red Barrel Roll action. If you perform a red Barrel Roll action, expose 1 damage card if able.",
                HostShip
            );
        }

        private void RegisterExposeDamageCardTrigger(GenericAction action)
        {
            HostShip.OnActionIsPerformed -= RegisterExposeDamageCardTrigger;
            if (action is BarrelRollAction && HostShip.Damage.GetFacedownCards().Count > 0)
            {
                RegisterAbilityTrigger(
                    TriggerTypes.OnFreeAction,
                    delegate
                    {
                        Messages.ShowInfo(HostShip.PilotInfo.PilotName + " has performed a red Barrel Roll action and must expose one damage card.");
                        HostShip.Damage.ExposeRandomFacedownCard(Triggers.FinishTrigger);
                    }
                );                
            }
        }

        private void RegisterSpendChargeTrigger(GenericAction action, ref bool isFreeAction)
        {
            HostShip.BeforeActionIsPerformed -= RegisterSpendChargeTrigger;
            RegisterAbilityTrigger(
                TriggerTypes.OnFreeAction,
                delegate
                {
                    HostShip.SpendCharges(2);
                    Triggers.FinishTrigger();
                }
            );
        }

        private void CleanUp()
        {
            HostShip.BeforeActionIsPerformed -= RegisterSpendChargeTrigger;
            HostShip.OnActionIsPerformed -= RegisterExposeDamageCardTrigger;
            Triggers.FinishTrigger();
        }

        public void RegisterTriggerActivationPhase(GenericShip host)
        {
            if (HostShip.State.Charges > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementActivationStart, AskToUseAbilityActivationPhase);
            }
        }

        private void AskToUseAbilityActivationPhase(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                NeverUseByDefault,
                TurnOnIgnoreObstaclesActivationPhase,
                descriptionLong: "Do you want to spend 1 Charge to ignore obstacles during this maneuver?",
                imageHolder: HostShip
            );
        }

        private void TurnOnIgnoreObstaclesActivationPhase(object sender, EventArgs e)
        {
            HostShip.SpendCharge();
            HostShip.IsIgnoreObstacles = true;
            HostShip.IsIgnoreObstacleObstructionWhenAttacking = true;
            Phases.Events.OnActivationPhaseEnd_NoTriggers += TurnOffIgnoreObstaclesActivationPhase;
            DecisionSubPhase.ConfirmDecision();
        }

        private void TurnOffIgnoreObstaclesActivationPhase()
        {
            HostShip.IsIgnoreObstacles = false;
            HostShip.IsIgnoreObstacleObstructionWhenAttacking = false;
            Phases.Events.OnActivationPhaseEnd_NoTriggers -= TurnOffIgnoreObstaclesActivationPhase;
        }

    }
}