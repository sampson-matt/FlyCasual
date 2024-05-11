using Upgrade;
using System.Collections.Generic;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class Outmaneuver : GenericUpgrade, IVariableCost
    {
        public Outmaneuver() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Outmaneuver",
                UpgradeType.Talent,
                cost: 6,
                abilityType: typeof(Abilities.SecondEdition.OutmaneuverAbilitySE)
            );
        }

        public void UpdateCost(GenericShip ship)
        {
            Dictionary<int, int> initiativeToCost = new Dictionary<int, int>()
            {
                {0, 5},
                {1, 5},
                {2, 5},
                {3, 5},
                {4, 6},
                {5, 6},
                {6, 6}
            };

            UpgradeInfo.Cost = initiativeToCost[ship.PilotInfo.Initiative];
        }
    }
}

namespace Abilities.SecondEdition
{
    public class OutmaneuverAbilitySE : Abilities.FirstEdition.OutmaneuverAbility
    {
        protected override bool AbilityCanBeUsed()
        {
            if (!(Combat.ArcForShot is Arcs.ArcFront)) return false;

            BoardTools.ShotInfo reverseShotInfo = new BoardTools.ShotInfo(Combat.Defender, Combat.Attacker, Combat.Defender.PrimaryWeapons);
            if (reverseShotInfo.InArc) return false;

            return true;
        }
    }
}