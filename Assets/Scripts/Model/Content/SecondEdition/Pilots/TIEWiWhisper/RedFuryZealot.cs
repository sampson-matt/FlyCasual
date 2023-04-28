using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEWiWhisperModifiedInterceptor
    {
        public class RedFuryZealot : TIEWiWhisperModifiedInterceptor
        {
            public RedFuryZealot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Red Fury Zealot",
                    2,
                    41,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent }
                );
            }
        }
    }
}
