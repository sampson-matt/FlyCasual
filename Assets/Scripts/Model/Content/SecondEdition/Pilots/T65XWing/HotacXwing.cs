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
                    3,
                    38,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Modification},
                    seImageNumber: 10
                );
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
            }
        }
    }
}
