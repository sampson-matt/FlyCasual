using Abilities.SecondEdition;
using ActionsList;
using Content;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class SoontirFelBoELSL : TIEInterceptor
        {
            public SoontirFelBoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Soontir Fel",
                    6,
                    49,
                    isLimited: true,
                    abilityType: typeof(SoontirFelBattleOverEndorAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.LsL
                    },
                    charges: 2,
                    extraUpgradeIcon: UpgradeType.Talent
                );
                PilotNameCanonical = "soontirfel-battleoverendor-lsl";
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());
                ModelInfo.SkinName = "Red Stripes";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you perform an attack, you may spend 1 Charge and gain 1 deplete token to boost or barrel roll.
    public class SoontirFelBattleOverEndorAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnAttackFinishAsAttacker += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackFinishAsAttacker -= RegisterAbility;
        }

        private void RegisterAbility(GenericShip ship)
        {
            if(HostShip.State.Charges > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, AskToUseAbility);
            }
            
        }

        private void AskToUseAbility(object sender, EventArgs e)
        {
            HostShip.BeforeActionIsPerformed += RegisterSpendChargeTrigger;
            CameraScript.RestoreCamera();
            HostShip.AskPerformFreeAction(
                new List<ActionsList.GenericAction>()
                {
                    new ActionsList.BoostAction(),
                    new ActionsList.BarrelRollAction()
                },
                CleanUp,
                HostShip.PilotInfo.PilotName,
                "After you perform an attack, you may spend 1 Charge and gain 1 deplete to perform a Barrel Roll or Boost action.",
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
                    HostShip.Tokens.AssignToken(typeof(DepleteToken), Triggers.FinishTrigger);
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