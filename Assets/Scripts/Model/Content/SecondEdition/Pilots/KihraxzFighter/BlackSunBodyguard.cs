using Ship;
using SubPhases;
using System;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.KihraxzFighter
    {
        public class BlackSunBodyguard : KihraxzFighter
        {
            public BlackSunBodyguard() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Black Sun Bodyguard",
                    4,
                    40,
                    limited: 2,
                    charges: 2,
                    extraUpgradeIcon: UpgradeType.Talent,
                    abilityType: typeof(Abilities.SecondEdition.BlackSunBodyguardAbility)
                );
                PilotNameCanonical = "blacksunbodyguard-wat1";

            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //Setup: Lose 2 charge.
    //After you suffer damage, recover 1 charge
    //Before you engage, you may spend 2 charges to recover 1 charge on 1 of you equiped upgrades
    public class BlackSunBodyguardAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            HostShip.OnSetupPlaced += OnSetupPlaced;
            HostShip.OnShieldLost += RegisterRecoverChargeShield;
            HostShip.OnDamageCardIsDealt += RegisterRecoverChargeHull;
            HostShip.OnCombatActivation += TryRegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnSetupPlaced -= OnSetupPlaced;
            HostShip.OnShieldLost -= RegisterRecoverChargeShield;
            HostShip.OnDamageCardIsDealt -= RegisterRecoverChargeHull;
            HostShip.OnCombatActivation -= TryRegisterAbility;
        }

        private void TryRegisterAbility(GenericShip ship)
        {
            if (HostShip.State.Charges > 1 && HasRechargeableUpgrades())
            {
                RegisterAbilityTrigger(TriggerTypes.OnCombatActivation, AskToUseOwnAbility);
            }
        }

        private void AskToUseOwnAbility(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                AlwaysUseByDefault,
                SelectAbilityTarget,
                descriptionLong: "Do you want to spend 2 charges to recover 1 charge on 1 of your equipped upgrades?",
                imageHolder: HostShip
            );
        }

        private void SelectAbilityTarget(object sender, EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();
            var phase = Phases.StartTemporarySubPhaseNew<BlackSunBodyguardDecisionSubphase>(
                "Black Sun Bodyguard: Select upgrade to recover 1 charge",
                Phases.CurrentSubPhase.CallBack);
            phase.HostShip = HostShip;
            phase.Start();
        }

        private bool HasRechargeableUpgrades()
        {
            return HostShip.UpgradeBar.GetRechargableUpgrades().Any();
        }

        private void RegisterRecoverChargeShield()
        {
            RegisterAbilityTrigger(TriggerTypes.OnShieldIsLost, RecoverChargeToken);
        }

        private void RegisterRecoverChargeHull(GenericShip ship)
        {
            RegisterAbilityTrigger(TriggerTypes.OnDamageCardIsDealt, RecoverChargeToken);
        }

        private void RecoverChargeToken(object sender, System.EventArgs e)
        {
            if (HostShip.State.Charges < HostShip.State.MaxCharges)
            {
                HostShip.RestoreCharges(1);
            }
            Triggers.FinishTrigger();
        }

        private void OnSetupPlaced(GenericShip ship)
        {
            HostShip.SpendCharges(2);
        }

        protected class BlackSunBodyguardDecisionSubphase : DecisionSubPhase
        {
            public GenericShip HostShip;

            public override void PrepareDecision(Action callBack)
            {
                DescriptionShort = "Black Sun Bodyguard";
                DescriptionLong = "Select upgrade to recover 1 charge";
                ImageSource = HostShip;

                DecisionViewType = DecisionViewTypes.ImagesUpgrade;

                foreach (var upgrade in HostShip.UpgradeBar.GetRechargableUpgrades().ToList())
                {
                    AddDecision(upgrade.UpgradeInfo.Name, delegate { RecoverCharge(upgrade); }, upgrade.ImageUrl);
                }

                DefaultDecisionName = GetDecisions().First().Name;

                ShowSkipButton = true;

                callBack();
            }

            private void RecoverCharge(GenericUpgrade upgrade)
            {
                upgrade.State.RestoreCharge();
                HostShip.SpendCharges(2);
                ConfirmDecision();
            }
        }
    }
}

