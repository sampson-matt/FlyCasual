using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections;
using System.Collections.Generic;
using Upgrade;

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
                    31,
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Illicit, UpgradeType.Pilot }
                );
            }
        }
    }
}
