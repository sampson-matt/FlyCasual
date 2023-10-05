using Upgrade;
using Ship;
using System.Collections.Generic;

namespace UpgradesList.SecondEdition
{
    public class R5D8 : GenericUpgrade
    {
        public R5D8() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "R5-D8",
                UpgradeType.Astromech,
                cost: 6,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.R5AstromechAbility),
                restriction: new FactionRestriction(Faction.Rebel),
                charges: 3
            );
        }
        public void UpdateCost(GenericShip ship)
        {
            Dictionary<int, int> agilityToCost = new Dictionary<int, int>()
            {
                {0, 2},
                {1, 3},
                {2, 4},
                {3, 6}
            };

            UpgradeInfo.Cost = agilityToCost[ship.ShipInfo.Agility];
        }
    }
}