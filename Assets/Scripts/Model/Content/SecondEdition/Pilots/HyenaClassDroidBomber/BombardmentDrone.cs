﻿using System;
using System.Collections.Generic;
using System.Linq;
using BoardTools;
using Upgrade;

namespace Ship.SecondEdition.HyenaClassDroidBomber
{
    public class BombardmentDrone : HyenaClassDroidBomber
    {
        public BombardmentDrone()
        {
            PilotInfo = new PilotCardInfo(
                "Bombardment Drone",
                3,
                31,
                limited: 3,
                abilityType: typeof(Abilities.SecondEdition.BombardmentDroneAbility),
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Sensor, UpgradeType.Device, UpgradeType.Device },
                pilotTitle: "Time on Target"
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BombardmentDroneAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnGetAvailableBombLaunchTemplates += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnGetAvailableBombLaunchTemplates -= CheckAbility;
        }

        private void CheckAbility(List<ManeuverTemplate> availableTemplates, GenericUpgrade upgrade)
        {
            List<ManeuverTemplate> dropTemplates = HostShip.GetAvailableBombDropTemplates(upgrade);

            foreach (ManeuverTemplate template in dropTemplates)
            {
                if (!availableTemplates.Any(n => n.Name == template.Name))
                {
                    availableTemplates.Add(template);
                }
            }
        }
    }
}