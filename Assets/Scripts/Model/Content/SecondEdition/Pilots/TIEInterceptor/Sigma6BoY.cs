using Abilities.SecondEdition;
using SubPhases;
using Upgrade;
using Ship;
using System.Linq;
using Abilities;
using ActionsList;
using Content;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class Sigma6 : TIEInterceptor
        {
            public Sigma6() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sigma6",
                    4,
                    43,
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Sigma6Ability),
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
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/sigma6-boy.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class Sigma6Ability : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully -= RegisterAbility;
        }

        protected void RegisterAbility(GenericShip ship)
        {
            if (HostShip.State.Charges > 0 && HostShip.RevealedManeuver.Speed >= 3 && HostShip.RevealedManeuver.Speed <= 5)
            {
                Triggers.RegisterTrigger(
                    new Trigger()
                    {
                        Name = HostShip.PilotInfo.PilotName + ": Free Slam action",
                        TriggerOwner = ship.Owner.PlayerNo,
                        TriggerType = TriggerTypes.OnMovementFinish,
                        EventHandler = PerformFreeFocusAction
                    }
                );
            }
        }

        private void PerformFreeFocusAction(object sender, System.EventArgs e)
        {
            HostShip.BeforeActionIsPerformed += RegisterSpendChargeTrigger;
            HostShip.AskPerformFreeAction(
                new SlamAction(true),
                CleanUp,
                HostShip.PilotInfo.PilotName,
                "After you fully execute a speed 3-5 maneuver you may spend 1 Charge to perform a Slam action.",
                HostShip
            );
        }

        private void RegisterSpendChargeTrigger(GenericAction action, ref bool isFreeAction)
        {
            HostShip.BeforeActionIsPerformed -= RegisterSpendChargeTrigger;
            RegisterAbilityTrigger(
                TriggerTypes.OnFreeAction,
                delegate
                {
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