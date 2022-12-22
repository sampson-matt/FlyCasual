using Abilities.SecondEdition;
using SubPhases;
using Upgrade;
using Ship;
using System.Linq;
using System.Collections.Generic;
using ActionsList;
using Content;
using System;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class Sigma4 : TIEInterceptor
        {
            public Sigma4() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sigma4",
                    4,
                    42,
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Sigma4Ability),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ShipInfo.Hull++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/sigma4-boy.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you perform a barrel roll action, you may spend a charge to perform a  boost action.
    public class Sigma4Ability : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += CheckConditions;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= CheckConditions;
        }

        private void CheckConditions(GenericAction action)
        {
            if (action is BarrelRollAction && HostShip.State.Charges > 0)
            {
                HostShip.OnActionDecisionSubphaseEnd += PerformBoostAction;
            }
        }

        private void PerformBoostAction(GenericShip ship)
        {
            HostShip.OnActionDecisionSubphaseEnd -= PerformBoostAction;
            HostShip.BeforeActionIsPerformed += PayCost;

            RegisterAbilityTrigger(TriggerTypes.OnFreeAction, AskPerformPerositionAction);
        }

        private void AskPerformPerositionAction(object sender, System.EventArgs e)
        {
            HostShip.AskPerformFreeAction(
                new BoostAction(),
                CleanUp,
                descriptionShort: Name,
                descriptionLong: "After you perform a Barrel Roll action, you may spend a charge to perform Boost action"
            );
        }

        private void PayCost(GenericAction action, ref bool isFreeAction)
        {
            HostShip.BeforeActionIsPerformed -= PayCost;
            RegisterAbilityTrigger(TriggerTypes.BeforeActionIsPerformed, SpendCharge);
        }

        private void SpendCharge(object sender, EventArgs e)
        {
            HostShip.SpendCharge();
            Triggers.FinishTrigger();
        }

        private void CleanUp()
        {
            HostShip.BeforeActionIsPerformed -= PayCost;
            Triggers.FinishTrigger();
        }
    }
}