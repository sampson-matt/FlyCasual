using Ship;
using Upgrade;
using ActionsList;
using System;
using System.Collections.Generic;
using Obstacles;
using System.Linq;

namespace UpgradesList.SecondEdition
{
    public class SensorJammer : GenericUpgrade
    {
        public SensorJammer() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Sensor Jammer",
                UpgradeType.Sensor,
                cost: 8,
                abilityType: typeof(Abilities.FirstEdition.SensorJammerAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/sensor/sensorjammer.png";
        }        
    }
}