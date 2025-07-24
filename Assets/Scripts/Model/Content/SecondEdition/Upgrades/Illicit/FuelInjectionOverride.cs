using ActionsList;
using BoardTools;
using Ship;
using SubPhases;
using System;
using Upgrade;
using Tokens;

namespace UpgradesList.SecondEdition
{
    public class FuelInjectionOverride : GenericUpgrade
    {
        public FuelInjectionOverride() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Fuel Injection Override",
                UpgradeType.Illicit,
                cost: 2,
                charges: 1,
                abilityType: typeof(Abilities.SecondEdition.FuelInjectionOverrideAbility)
            );

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/RSLUpgrades/fuelinjectionoverride.jpg";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class FuelInjectionOverrideAbility : GenericAbility
    {
        private GenericAction Action;
        public override void ActivateAbility()
        {
            HostShip.OnMovementActivationStart += RegisterTrigger;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementActivationStart -= RegisterTrigger;
        }

        private void RegisterTrigger(GenericShip ship)
        {
            if (HostUpgrade.State.Charges > 0)
            {
                Triggers.RegisterTrigger(new Trigger()
                {
                    Name = Name,
                    TriggerType = TriggerTypes.OnMovementActivationStart,
                    TriggerOwner = HostShip.Owner.PlayerNo,
                    EventHandler = AskUseFuelInjectionOverride
                });
            }
        }

        private void AskUseFuelInjectionOverride(object sender, System.EventArgs e)
        {
            if (HostUpgrade.State.Charges > 0)
            {
                AskToUseAbility(
                    HostUpgrade.UpgradeInfo.Name,
                    NeverUseByDefault,
                    ActivateFuelInjectionOverride,
                    descriptionLong: "Do you want to spend 1 Charge? (If you do, until the end of the round, while you move, you must use a template of 1 speed higher, if able.)",
                    imageHolder: HostUpgrade
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        public void ActivateFuelInjectionOverride(object sender, System.EventArgs e)
        {
            Phases.Events.OnEndPhaseStart_NoTriggers += DeactivateActivateFuelInjectionOverrideAbility;

            PayActivationCost(UseHigherTemplates);
        }

        protected virtual void PayActivationCost(Action callback)
        {
            HostUpgrade.State.SpendCharge();
            HostShip.Tokens.AssignToken(typeof(StrainToken), callback);
        }

        private void UseHigherTemplates()
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();

            Messages.ShowInfo(HostUpgrade.UpgradeInfo.Name + " allows " + HostShip.PilotInfo.PilotName + " to perform actions and red maneuvers even while stressed");

            HostShip.OnUpdateChosenBoostTemplate += UpdateBoostTemplate;
            HostShip.OnUpdateChosenBarrelRollTemplate += UpdateBarrelRollTemplate;
            HostShip.BeforeMovementIsExecuted += UpdateMovementTemplate;

            Triggers.FinishTrigger();
        }

        

        public void DeactivateActivateFuelInjectionOverrideAbility()
        {
            Phases.Events.OnEndPhaseStart_NoTriggers -= DeactivateActivateFuelInjectionOverrideAbility;

            HostShip.OnUpdateChosenBoostTemplate -= UpdateBoostTemplate;
            HostShip.OnUpdateChosenBarrelRollTemplate -= UpdateBarrelRollTemplate;
            HostShip.BeforeMovementIsExecuted -= UpdateMovementTemplate;
        }

        private void UpdateMovementTemplate(GenericShip ship)
        {
            ship.AssignedManeuver.TryIncreaseSpeed();
        }

        private void UpdateBoostTemplate(ref string name)
        {

            HostShip.OnUpdateChosenBoostTemplate -= UpdateBoostTemplate;
            bool isChanged = false;

            if (name.Contains("1"))
            {
                name = name.Replace('1', '2');
                isChanged = true;
            }

            if (isChanged)
            {
                Messages.ShowInfo("Fuel Injection Override: Template of 1 speed higher is used");
            }
        }

        private void UpdateBarrelRollTemplate(ref ManeuverTemplate maneuverTemplate)
        {
            HostShip.OnUpdateChosenBarrelRollTemplate -= UpdateBarrelRollTemplate;
            if (maneuverTemplate.TryIncreaseSpeed())
            {
                Messages.ShowInfo("Fuel Injection Override: Template of 1 speed higher is used");
            }
        }
    }
}