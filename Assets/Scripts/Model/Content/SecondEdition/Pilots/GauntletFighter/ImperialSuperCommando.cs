
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class ImperialSuperCommando : GauntletFighter
        {
            public ImperialSuperCommando() : base()
            {
                //IsWIP = true;

                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo
                (
                    "Imperial Super Commando",
                    2,
                    51,
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent},
                    factionOverride: Faction.Imperial
                );

                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Title);

                ImageUrl = "https://infinitearenas.com/xw2legacy/images/pilots/imperialsupercommando.png";
            }
        }
    }
}