using Upgrade;
using Ship;
using System.Collections.Generic;

namespace UpgradesList.SecondEdition
{
    public class StealthDevice : GenericUpgrade, IVariableCost
    {
        public StealthDevice() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Stealth Device",
                UpgradeType.Modification,
                cost: 3,
                charges: 1,
                abilityType: typeof(Abilities.SecondEdition.StealthDeviceAbility)
            );
        }

        public void UpdateCost(GenericShip ship)
        {
            Dictionary<int, int> agilityToCost = new Dictionary<int, int>()
            {
                {0, 3},
                {1, 4},
                {2, 6},
                {3, 8}
            };

            UpgradeInfo.Cost = agilityToCost[ship.ShipInfo.Agility];
        }
    }
}

namespace Abilities.SecondEdition
{
    public class StealthDeviceAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.AfterGotNumberOfDefenceDice += CheckDefenseBonus;
            HostShip.OnDamageWasSuccessfullyDealt += RegisterStealthDeviceCleanup;
        }

        public override void DeactivateAbility()
        {
            HostShip.AfterGotNumberOfDefenceDice -= CheckDefenseBonus;
            HostShip.OnDamageWasSuccessfullyDealt -= RegisterStealthDeviceCleanup;
        }

        private void CheckDefenseBonus(ref int count)
        {
            if (HostUpgrade.State.Charges > 0)
            {
                Messages.ShowInfo($"{HostUpgrade.UpgradeInfo.Name}: {HostShip.PilotInfo.PilotName} rolls 1 additional defense die");
                count++;
            }
        }

        private void RegisterStealthDeviceCleanup(GenericShip ship, bool isCritical)
        {
            Triggers.RegisterTrigger(new Trigger
            {
                Name = "Discard Stealth Device",
                TriggerType = TriggerTypes.OnDamageWasSuccessfullyDealt, //Stealth Device in SE is deactivated on damage taken
                TriggerOwner = HostShip.Owner.PlayerNo,
                EventHandler = StealthDeviceCleanup
            });
        }

        protected void StealthDeviceCleanup(object sender, System.EventArgs e)
        {
            Messages.ShowInfo("Stealth Device: This ship has suffered a hit! Lose 1 charge on Stealth Device");
            HostUpgrade.State.SpendCharge();
            Triggers.FinishTrigger();
        }
    }
}