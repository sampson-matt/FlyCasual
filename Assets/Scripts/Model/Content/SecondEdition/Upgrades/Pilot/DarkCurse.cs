using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class DarkCursePilotAbility : GenericUpgrade
    {
        public DarkCursePilotAbility() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Dark Curse Pilot Ability",
                UpgradeType.Pilot,
                cost: 12,
                abilityType: typeof(Abilities.FirstEdition.DarkCurseAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Imperial/darkcurse.png";
        }


    }
}