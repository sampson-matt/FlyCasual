using BoardTools;
using Movement;
using System.Collections.Generic;
using System.Linq;
using Upgrade;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class TrajectorySimulator : GenericUpgrade, IVariableCost
    {
        public TrajectorySimulator() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Trajectory Simulator",
                UpgradeType.Sensor,
                cost: 6,
                abilityType: typeof(Abilities.SecondEdition.TrajectorySimulatorAbility)
            );
        }
        public void UpdateCost(GenericShip ship)
        {
            Dictionary<BaseSize, int> sizeToCost = new Dictionary<BaseSize, int>()
            {
                {BaseSize.Small, 5},
                {BaseSize.Medium, 4},
                {BaseSize.Large, 3},
            };

            UpgradeInfo.Cost = sizeToCost[ship.ShipInfo.BaseSize];
        }
    }    
}

namespace Abilities.SecondEdition
{
    public class TrajectorySimulatorAbility : Abilities.FirstEdition.TrajectorySimulatorAbility
    {
        protected override void TrajectorySimulatorTemplate(List<ManeuverTemplate> availableTemplates, GenericUpgrade upgrade)
        {
            if (Phases.CurrentPhase.GetType() != typeof(MainPhases.SystemsPhase)) return;

            if (upgrade.UpgradeInfo.SubType != UpgradeSubType.Bomb) return;

            ManeuverTemplate newTemplate = new ManeuverTemplate(ManeuverBearing.Straight, ManeuverDirection.Forward, ManeuverSpeed.Speed5);

            if (!availableTemplates.Any(t => t.Name == newTemplate.Name))
            {
                availableTemplates.Add(newTemplate);
            }
        }
    }
}