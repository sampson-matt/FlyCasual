using Actions;
using ActionsList;
using Ship;
using System.Collections.Generic;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class AngledDeflectors : GenericUpgrade, IVariableCost
    {
        public AngledDeflectors() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Angled Deflectors",
                UpgradeType.Modification,
                cost: 6,
                restrictions: new UpgradeCardRestrictions(
                    new BaseSizeRestriction(BaseSize.Small, BaseSize.Medium), 
                    new StatValueRestriction(
                        StatValueRestriction.Stats.Shields,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        1
                    )
                ),
                addAction: new ActionInfo(typeof(ReinforceAction)),
                addShields: -1
            );
        }

        public void UpdateCost(GenericShip ship)
        {
            Dictionary<int, int> agilityToCost = new Dictionary<int, int>()
            {
                {0, 6},
                {1, 2},
                {2, 1},
                {3, 1}
            };

            UpgradeInfo.Cost = agilityToCost[ship.ShipInfo.Agility];
        }
    }
}