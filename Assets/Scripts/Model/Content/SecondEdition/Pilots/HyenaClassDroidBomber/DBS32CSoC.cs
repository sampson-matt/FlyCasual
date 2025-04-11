using Actions;
using ActionsList;
using Content;
using Ship;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship.SecondEdition.HyenaClassDroidBomber
{
    public class DBS32CSoC : HyenaClassDroidBomber
    {
        public DBS32CSoC()
        {
            PilotInfo = new PilotCardInfo(
                "DBS-32C",
                3,
                30,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.DBS32CSoCAbility),
                charges: 2,
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.LsL,
                    Tags.Droid
                },
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Sensor, UpgradeType.TacticalRelay },
                pilotTitle: "Siege of Coruscant"
            );

            ShipInfo.ActionIcons.RemoveActions(typeof(ReloadAction));
            ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(JamAction), ActionColor.Red));

            PilotNameCanonical = "dbs32c-siegeofcoruscant-lsl";
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you perform a calculate action, you may spend a charge to perform a jam action.
    public class DBS32CSoCAbility : GenericAbility
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
            if (action is CalculateAction && HostShip.State.Charges > 0)
            {
                HostShip.OnActionDecisionSubphaseEnd += PerformJamAction;
            }
        }

        private void PerformJamAction(GenericShip ship)
        {
            HostShip.OnActionDecisionSubphaseEnd -= PerformJamAction;
            HostShip.BeforeActionIsPerformed += PayCost;

            RegisterAbilityTrigger(TriggerTypes.OnFreeAction, AskPerformPerositionAction);
        }

        private void AskPerformPerositionAction(object sender, System.EventArgs e)
        {
            HostShip.AskPerformFreeAction(
                new JamAction(),
                CleanUp,
                descriptionShort: Name,
                descriptionLong: "After you perform a Calculate action, you may spend a charge to perform a Jam action"
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