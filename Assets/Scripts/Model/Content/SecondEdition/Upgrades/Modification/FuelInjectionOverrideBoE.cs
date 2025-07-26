using ActionsList;
using BoardTools;
using SubPhases;
using System;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class FuelInjectionOverrideBoE : GenericUpgrade
    {
        public FuelInjectionOverrideBoE() : base()
        {
            IsHidden = true;

            UpgradeInfo = new UpgradeCardInfo(
                "Fuel Injection Override",
                UpgradeType.Modification,
                cost: 0,
                charges: 2,
                abilityType: typeof(Abilities.SecondEdition.FuelInjectionOverrideBoEAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/BattleOverEndor/FuelInjectionOverrideBoE.jpg";
            NameCanonical = "fuelinjectionoverride-battleoverendore";
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class FuelInjectionOverrideBoEAbility : GenericAbility
    {

        private GenericAction Action;
        public override void ActivateAbility()
        {
            HostShip.BeforeActionIsPerformed += RegisterFuelInjectionOverrideAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.BeforeActionIsPerformed -= RegisterFuelInjectionOverrideAbility;
        }

        private void RegisterFuelInjectionOverrideAbility(GenericAction action, ref bool data)
        {
            if (HostUpgrade.State.Charges > 0 && (action is BoostAction || action is BarrelRollAction))
            {
                Action = action;
                RegisterAbilityTrigger(TriggerTypes.BeforeActionIsPerformed, AskToUseFuelInjectionOverrideAbility);
            }
        }

        private void AskToUseFuelInjectionOverrideAbility(object sender, System.EventArgs e)
        {
            AskToUseAbility
            (
                descriptionShort: HostUpgrade.UpgradeInfo.Name,
                descriptionLong: "Do you want to spend a charge to use a template with a speed 1 higher?",
                useByDefault: NeverUseByDefault,
                useAbility: UpdateTemplate,
                callback: Cleanup,
                imageHolder: HostUpgrade,
                showSkipButton: false
            );
        }

        private void Cleanup()
        {
            HostUpgrade.State.SpendCharge();
            Triggers.FinishTrigger();
        }

        private void UpdateTemplate(object sender, EventArgs e)
        {
            DecisionSubPhase.ConfirmDecision();
            if (Action is BoostAction)
            {
                HostShip.OnUpdateChosenBoostTemplate += UpdateBoostTemplate;
            }
            if (Action is BarrelRollAction)
            {
                HostShip.OnUpdateChosenBarrelRollTemplate += UpdateBarrelRollTemplate;
            }
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

