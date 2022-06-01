using Upgrade;
using System.Collections.Generic;
using Ship;
using System.Linq;
using System;

namespace UpgradesList.SecondEdition
{
    public class LoneWolfHotac : GenericUpgrade
    {
        public LoneWolfHotac() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);
            UpgradeInfo = new UpgradeCardInfo(
                "Lone Wolf Hotac",
                UpgradeType.Talent,
                cost: 5,
                abilityType: typeof(Abilities.FirstEdition.LoneWolfAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/talent/lonewolf.png";
        }        
    }
}