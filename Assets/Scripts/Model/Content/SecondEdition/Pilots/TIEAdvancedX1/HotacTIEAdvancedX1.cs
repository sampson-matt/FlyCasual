using System.Collections.Generic;
using Mods.ModsList;
using System;
using Upgrade;
namespace Ship
{
    namespace SecondEdition.TIEAdvancedX1
    {
        public class HotacTIEAdvancedX1 : TIEAdvancedX1
        {
            public HotacTIEAdvancedX1() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac Tie Advanced X1",
                    1,
                    0,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Pilot},
                    seImageNumber: 98
                );
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
            }
        }
    }
}