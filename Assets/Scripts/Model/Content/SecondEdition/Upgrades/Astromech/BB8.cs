using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class BB8 : GenericUpgrade, IVariableCost
    {
        public BB8() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "BB-8",
                UpgradeType.Astromech,
                charges: 2,
                cost: 8,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Resistance),
                abilityType: typeof(Abilities.SecondEdition.BB8Ability)
            );
        }


        public void UpdateCost(GenericShip ship)
        {
            Dictionary<int, int> initiativeToCost = new Dictionary<int, int>()
            {
                {0, 2},
                {1, 2},
                {2, 3},
                {3, 4},
                {4, 4},
                {5, 5},
                {6, 6}
            };

            UpgradeInfo.Cost = initiativeToCost[ship.PilotInfo.Initiative];
        }
    }
}

namespace Abilities.SecondEdition
{
    //Before you execute a blue maneuver, you may spend 1 charge to perform a barrel roll or boost action.
    public class BB8Ability : BBAstromechAbility
    {
        public BB8Ability()
        {
            AbilityActions = new List<GenericAction> { new BarrelRollAction(), new BoostAction() };
        }
    }
}