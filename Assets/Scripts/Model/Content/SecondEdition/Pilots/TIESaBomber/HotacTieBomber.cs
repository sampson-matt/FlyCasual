using System.Collections.Generic;
using Mods.ModsList;
using System;
using Upgrade;
namespace Ship
{
    namespace SecondEdition.TIESaBomber
    {
        public class HotacBomberPilot : TIESaBomber, TIE
        {
            public HotacBomberPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac Bomber Pilot",
                    1,
                    0,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Pilot, UpgradeType.Torpedo},
                    seImageNumber: 112
                );
                RequiredMods = new List<Type>() { typeof(HotacEliteImperialPilotsModSE) };
            }
        }
    }
}
