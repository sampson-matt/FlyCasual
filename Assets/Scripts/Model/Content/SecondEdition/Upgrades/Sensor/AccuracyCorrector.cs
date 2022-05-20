using Ship;
using Upgrade;
using ActionsList;
using System;
using System.Collections.Generic;
using Obstacles;
using System.Linq;

namespace UpgradesList.SecondEdition
{
    public class AccuracyCorrector : GenericUpgrade
    {
        public AccuracyCorrector() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Accuracy Corrector",
                UpgradeType.Sensor,
                cost: 6,
                abilityType: typeof(Abilities.FirstEdition.AccuracyCorrectorAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/sensor/accuracycorrector.png";
        }        
    }
}