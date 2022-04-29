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
                    0,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Modification, UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Modification, UpgradeType.Talent, UpgradeType.Pilot },
                    seImageNumber: 18
                );
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) }; 
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Missile);
                ShipInfo.UpgradeIcons.Upgrades.Add(UpgradeType.Gunner);
            }
        }
    }
}
