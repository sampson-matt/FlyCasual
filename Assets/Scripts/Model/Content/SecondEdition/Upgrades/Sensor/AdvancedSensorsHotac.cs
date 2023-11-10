using Ship;
using Upgrade;
using ActionsList;
using System;
using System.Collections.Generic;
using Obstacles;
using System.Linq;

namespace UpgradesList.SecondEdition
{
    public class AdvancedSensorsHotac : GenericUpgrade
    {
        public AdvancedSensorsHotac() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Advanced Sensors",
                UpgradeType.Sensor,
                cost: 10,
                abilityType: typeof(Abilities.SecondEdition.AdvancedSensorsHotacAbility)
            );
            NameCanonical = "advancedsensorshotac";
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/sensor/advancedsensors.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class AdvancedSensorsHotacAbility : GenericAbility
    {
        // Do not skip Action Selection after a collision with a ship or obstacle
        // or after executing a red maneuver
        List<GenericObstacle> IgnoredObstacles = new List<GenericObstacle>();

        public override void ActivateAbility()
        {
            HostShip.CanPerformActionsWhenBumped = true;
            HostShip.CanPerformActionsWhenOverlapping = true;
            HostShip.OnCheckCanPerformActionsWhileStressed += ConfirmThatIsPossible;
            HostShip.OnCanPerformActionWhileStressed += CheckRedManeuver;
        }

        public override void DeactivateAbility()
        {
            HostShip.CanPerformActionsWhenBumped = false;
            HostShip.CanPerformActionsWhenOverlapping = false;
            HostShip.OnCheckCanPerformActionsWhileStressed -= ConfirmThatIsPossible;
            HostShip.OnCanPerformActionWhileStressed -= CheckRedManeuver;
        }

        private void ConfirmThatIsPossible(ref bool isAllowed)
        {
            isAllowed = (HostShip.AssignedManeuver.ColorComplexity == Movement.MovementComplexity.Complex);
        }

        private void CheckRedManeuver(GenericAction action, ref bool isAllowed)
        {
            isAllowed = (HostShip.AssignedManeuver.ColorComplexity == Movement.MovementComplexity.Complex);
        }


    }
}