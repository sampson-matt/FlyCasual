using System.Collections.Generic;
using Mods.ModsList;
using System;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class HotacXwing : T65XWing
        {
            public HotacXwing() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac X-Wing",
                    2,
                    0,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Talent, UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Pilot, UpgradeType.Pilot, UpgradeType.Modification, UpgradeType.Modification },
                    seImageNumber: 11
                );
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
            }
        }
    }
}
