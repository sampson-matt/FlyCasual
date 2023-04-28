using Ship;
using SubPhases;
using ActionsList;
using Actions;
using System;
using Tokens;
using Upgrade;

namespace Ship.SecondEdition.V19TorrentStarfighter
{
    public class Slammer : V19TorrentStarfighter
    {
        public Slammer()
        {
            PilotInfo = new PilotCardInfo(
                "\"Slammer\"",
                1,
                24,
                true,
                abilityType: typeof(Abilities.SecondEdition.SlammerAbility),
                pilotTitle: "Blue Three",
                charges: 2,
                regensCharges: 1,
                extraUpgradeIcon: UpgradeType.Talent
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you fully execute a speed 3-5 maneuver you may spend 1 charge to perform a boost action, even while stressed.
    public class SlammerAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship)
        {
            //AI doesn't use ability
            if (HostShip.Owner.UsesHotacAiRules) return;

            if (HostShip.State.Charges >= 2)
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, AskUseAbility);
            }
        }

        private void AskUseAbility(object sender, EventArgs e)
        {
            HostShip.BeforeActionIsPerformed += RegisterSpendChargeTrigger;
            HostShip.AskPerformFreeAction(
                new SlamAction(true) { CanBePerformedWhileStressed = true },
                CleanUp,
                HostShip.PilotInfo.PilotName,
                "After you fully execute a maneuver you may spend 1 Charge to perform a Slam action, even while stressed.",
                HostShip
            );
        }

        private void RegisterSpendChargeTrigger(GenericAction action, ref bool isFreeAction)
        {
            HostShip.BeforeActionIsPerformed -= RegisterSpendChargeTrigger;
            RegisterAbilityTrigger(
                TriggerTypes.OnFreeAction,
                delegate {
                    HostShip.SpendCharges(2);
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