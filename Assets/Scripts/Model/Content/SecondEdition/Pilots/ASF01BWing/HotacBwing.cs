using System.Collections;
using System.Collections.Generic;
using Mods.ModsList;
using System;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ASF01BWing
    {
        public class HotacBwing : ASF01BWing
        {
            public HotacBwing() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac B-Wing",
                    3,
                    5,
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Modification, UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Talent, UpgradeType.Pilot},
                    seImageNumber: 25
                );
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
                ModelInfo.SkinName = "Blue";
            }
        }
    }
}
