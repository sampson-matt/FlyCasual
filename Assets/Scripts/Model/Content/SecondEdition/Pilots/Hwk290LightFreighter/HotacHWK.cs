using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections;
using System.Collections.Generic;
using Upgrade;
using Mods.ModsList;

namespace Ship
{
    namespace SecondEdition.Hwk290LightFreighter
    {
        public class HotacHWK : Hwk290LightFreighter
        {
            public HotacHWK() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac HWK",
                    3,
                    0,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Illicit, UpgradeType.Pilot, UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Talent, UpgradeType.Sensor, UpgradeType.Modification, UpgradeType.Modification},
                    seImageNumber: 45
                );
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
            }
        }
    }
}
