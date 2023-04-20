using ActionsList;
using Ship;
using System.Collections.Generic;
using Tokens;
using Upgrade;
namespace UpgradesList.SecondEdition
{
    public class PrimedThrusters : GenericUpgrade, IVariableCost
    {
        public PrimedThrusters() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Primed Thrusters",
                UpgradeType.Tech,
                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.PrimedThrustersAbility)
            );
        }
        
        public void UpdateCost(GenericShip ship)
        {
            Dictionary<int, int> initiativeToCost = new Dictionary<int, int>()
            {
                {0, 4},
                {1, 5},
                {2, 6},
                {3, 7},
                {4, 8},
                {5, 9},
                {6, 10}
            };

            UpgradeInfo.Cost = initiativeToCost[ship.PilotInfo.Initiative];
        }
    }
}

namespace Abilities.SecondEdition
{
    public class PrimedThrustersAbility : GenericAbility
    {
        private bool set = false;

        public override void ActivateAbility()
        {
            HostShip.OnTokenIsAssigned += UsePrimedThruster;
            HostShip.OnTokenIsRemoved += UsePrimedThruster;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnTokenIsAssigned -= UsePrimedThruster;
            HostShip.OnTokenIsRemoved -= UsePrimedThruster;
        }

        private void UsePrimedThruster(GenericShip ship, GenericToken token)
        {
            if (token is StressToken)
            {
                if (!set && HostShip.Tokens.CountTokensByType(typeof(Tokens.StressToken)) <= 2)
                {
                    HostShip.ActionBar.ActionsThatCanbePreformedwhileStressed.Add(typeof(BoostAction));
                    HostShip.ActionBar.ActionsThatCanbePreformedwhileStressed.Add(typeof(BarrelRollAction));
                    set = true;
                }
                else if (set && HostShip.Tokens.CountTokensByType(typeof(Tokens.StressToken)) > 2)
                {
                    HostShip.ActionBar.ActionsThatCanbePreformedwhileStressed.Remove(typeof(BoostAction));
                    HostShip.ActionBar.ActionsThatCanbePreformedwhileStressed.Remove(typeof(BarrelRollAction));
                    set = false;
                }
            }
        }
    }
}