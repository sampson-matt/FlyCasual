﻿using Upgrade;
using UnityEngine;
using Ship;
using System.Collections.Generic;

namespace UpgradesList.SecondEdition
{
    public class R2D2 : GenericUpgrade, IVariableCost
    {
        public R2D2() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "R2-D2",
                UpgradeType.Astromech,
                cost: 4,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.R2AstromechAbility),
                restriction: new FactionRestriction(Faction.Rebel),
                charges: 3
            );
        }

        public void UpdateCost(GenericShip ship)
        {
            Dictionary<int, int> agilityToCost = new Dictionary<int, int>()
            {
                {0, 3},
                {1, 4},
                {2, 7},
                {3, 11}
            };

            UpgradeInfo.Cost = agilityToCost[ship.ShipInfo.Agility];
        }
    }
}