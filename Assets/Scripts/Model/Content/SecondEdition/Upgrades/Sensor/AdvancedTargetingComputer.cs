using Ship;
using Upgrade;
using ActionsList;
using System;
using System.Collections.Generic;
using Tokens;

namespace UpgradesList.SecondEdition
{
    public class AdvancedTargetingComputer : GenericUpgrade
    {
        public AdvancedTargetingComputer() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Advanced Targeting Computer",
                UpgradeType.Sensor,
                cost: 5,
                abilityType: typeof(Abilities.FirstEdition.AdvancedTargetingComputerAbility)
            );

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/sensor/advancedtargetingcomputer.png";
        }
    }
}