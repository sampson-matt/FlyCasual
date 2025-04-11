using ActionsList;
using Bombs;
using Content;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESaBomber
    {
        public class Scimitar3BoELSL : TIESaBomber
        {
            public Scimitar3BoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Scimitar 3",
                    4,
                    29,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.LsL
                    },
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Abilities.SecondEdition.Scimitar3Ability),
                    extraUpgradeIcon: UpgradeType.Talent
                );
                PilotNameCanonical = "scimitar3-battleoverendor-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you drop a bomb, you may spend 1 charge to perform a Boost action.
    public class Scimitar3Ability : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnBombWasDropped += OnDeviceDropped;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnBombWasDropped -= OnDeviceDropped;
        }

        private void OnDeviceDropped()
        {
            if(HostShip.State.Charges > 0 && BombsManager.CurrentDevice.UpgradeInfo.SubType == UpgradeSubType.Bomb)
            {
                RegisterAbilityTrigger(TriggerTypes.OnBombWasDropped, AskUseAbility);
            }
            
        }

        private void AskUseAbility(object sender, EventArgs e)
        {
            HostShip.BeforeActionIsPerformed += RegisterSpendChargeTrigger;
            HostShip.AskPerformFreeAction(
                new BoostAction(),
                CleanUp,
                HostShip.PilotInfo.PilotName,
                "After you drop a bomb, you may spend 1 Charge to perform a Boost action.",
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