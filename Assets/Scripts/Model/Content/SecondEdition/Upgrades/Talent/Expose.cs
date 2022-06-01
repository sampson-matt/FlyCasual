using Upgrade;
using System;
using Tokens;

namespace UpgradesList.SecondEdition
{
    public class Expose : GenericUpgrade
    {
        public Expose() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);
            UpgradeInfo = new UpgradeCardInfo(
                "Expose",
                UpgradeType.Talent,
                cost: 8,
                abilityType: typeof(Abilities.FirstEdition.ExposeAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/talent/expose.png";
        }        
    }
}