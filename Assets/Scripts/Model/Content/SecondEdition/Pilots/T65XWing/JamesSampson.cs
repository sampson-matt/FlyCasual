using System.Collections.Generic;
using Mods.ModsList;
using System;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class JamesSampson : T65XWing
        {
            public JamesSampson() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "James Sampson",
                    3,
                    38,
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Modification }
                );
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
                ImageUrl = "https://www.x-wing-cardcreator.com/img/published/James%20Sampson_sampsonmatt_0.png";
            }
        }
    }
}
