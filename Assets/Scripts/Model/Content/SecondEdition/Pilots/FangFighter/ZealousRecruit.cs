using Upgrade;
using Content;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.FangFighter
    {
        public class ZealousRecruit : FangFighter
        {
            public ZealousRecruit() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Zealous Recruit",
                    1,
                    41,
                    tags: new List<Tags>
                    {
                        Tags.Mandalorian
                    },
                    extraUpgradeIcon: UpgradeType.Modification
                );
                ModelInfo.SkinName = "Zealous Recruit";
            }
        }
    }
}