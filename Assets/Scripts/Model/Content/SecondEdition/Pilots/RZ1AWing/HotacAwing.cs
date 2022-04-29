using System.Collections;
using System.Collections.Generic;
using Mods.ModsList;
using System;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ1AWing
    {
        public class HotacAwingPilot : RZ1AWing
        {
            public HotacAwingPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac A-Wing",
                    3,
                    5,
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Modification, UpgradeType.Talent, UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Modification, UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Modification, },
                    seImageNumber: 21
                );
                
                ModelInfo.SkinName = "Green";
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
            }

        }
    }
}