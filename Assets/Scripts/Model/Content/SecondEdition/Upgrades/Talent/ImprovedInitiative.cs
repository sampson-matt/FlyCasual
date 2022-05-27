using Upgrade;
using System.Collections.Generic;
using Ship;
using System.Linq;

namespace UpgradesList.SecondEdition
{
    public class ImprovedInitiative : GenericUpgrade, IVariableCost
    {
        public ImprovedInitiative() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Improved Initiative",
                UpgradeType.Init,
                cost: 0,
                abilityType: typeof(Abilities.SecondEdition.ImprovedInitiativeAbility)
            );

            ImageUrl = "https://i.imgur.com/nvHEwLO.png";
        }
        public void UpdateCost(GenericShip ship)
        {
            Dictionary<int, int> initiativeToCost = new Dictionary<int, int>()
            {
                {2, 9},
                {3, 12},
                {4, 15},
                {5, 18}
            };

            if(initiativeToCost.ContainsKey(ship.PilotInfo.Initiative))
            {
                UpgradeInfo.Cost = initiativeToCost[ship.PilotInfo.Initiative];
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ImprovedInitiativeAbility : GenericAbility
    {
        private bool isActive = false;
        public override void ActivateAbility()
        {
            
        }

        public override void ActivateAbilityForSquadBuilder()
        {
            if(!isActive)
            {
                HostShip.PilotInfo.Initiative++;
                isActive = true;
                if(HostShip.PilotInfo.Initiative<6)
                {
                    HostShip.UpgradeBar.AddSlot(UpgradeType.Init);
                }
            }            
        }

        public override void DeactivateAbility()
        {
            if(isActive)
            {
                HostShip.PilotInfo.Initiative--;
                isActive = false;
                if(HostShip.PilotInfo.Initiative<5)
                {
                    HostShip.UpgradeBar.RemoveSlot(UpgradeType.Init);
                }
            }
            
        }

        public override void DeactivateAbilityForSquadBuilder()
        {
            if (isActive)
            {
                HostShip.PilotInfo.Initiative--;
                isActive = false;
                if (HostShip.PilotInfo.Initiative < 5)
                {
                    HostShip.UpgradeBar.RemoveSlot(UpgradeType.Init);
                }
            }
        }
    }
}