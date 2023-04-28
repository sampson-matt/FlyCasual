
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

                PilotInfo = new PilotCardInfo
                (
                    "Imperial Super Commando",
                    2,
                    51,
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent},
                    factionOverride: Faction.Imperial
                );

                ModelInfo.SkinName = "Gray";

                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Title);
            }
        }
    }
}