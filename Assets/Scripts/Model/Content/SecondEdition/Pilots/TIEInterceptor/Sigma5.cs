using Abilities.SecondEdition;
using SubPhases;
using Upgrade;
using Ship;
using System.Linq;
using Abilities;
using ActionsList;
using Actions;
using System;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class Sigma5 : TIEInterceptor
        {
            public Sigma5() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sigma5",
                    4,
                    43,
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Sigma5Ability),
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ShipInfo.Hull++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/sigma5-boy.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class Sigma5Ability : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnAttackHitAsAttacker += RegisterHitAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackHitAsAttacker -= RegisterHitAbility;
        }

        private void RegisterHitAbility()
        {
            if(HostShip.State.Charges>0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackHit, AskUseAbility);
            }            
        }

        private void AskUseAbility(object sender, EventArgs e)
        {
            HostShip.BeforeActionIsPerformed += RegisterSpendChargeTrigger;
            HostShip.AskPerformFreeAction(
                new EvadeAction(),
                CleanUp,
                HostShip.PilotInfo.PilotName,
                "After you perform an attack that hits you may spend 1 Charge to perform an Evade action.",
                HostShip
            );
        }

        private void RegisterSpendChargeTrigger(GenericAction action, ref bool isFreeAction)
        {
            HostShip.BeforeActionIsPerformed -= RegisterSpendChargeTrigger;
            RegisterAbilityTrigger(
                TriggerTypes.OnFreeAction,
                delegate {
                    HostShip.SpendCharge();
                    Triggers.FinishTrigger();
                }
            );
        }

        private void CleanUp()
        {
            HostShip.BeforeActionIsPerformed -= RegisterSpendChargeTrigger;
            Triggers.FinishTrigger();
        }
    }
}