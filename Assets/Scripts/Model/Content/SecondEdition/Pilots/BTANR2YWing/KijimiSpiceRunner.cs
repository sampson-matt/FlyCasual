using Content;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTANR2YWing
    {
        public class KijimiSpiceRunner : BTANR2YWing
        {
            public KijimiSpiceRunner() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Kijimi Spice Runner",
                    2,
                    30,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Illicit
                    },
                    tags: new List<Tags>
                    {
                        Tags.YWing
                    }
                );

                ModelInfo.SkinName = "Red";
            }
        }
    }
}
