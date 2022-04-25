using Upgrade;
using System.Collections.Generic;
using Mods.ModsList;
using System;

namespace Ship
{
    namespace SecondEdition.BTLA4YWing
    {
        public class HotacYwing : BTLA4YWing
        {
            public HotacYwing() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac Y-Wing",
                    2,
                    29,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Modification },
                    seImageNumber: 18
                );
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
            }
        }
    }
}
