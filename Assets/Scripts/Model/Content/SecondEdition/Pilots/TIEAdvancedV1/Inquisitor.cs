using System.Collections.Generic;
using Upgrade;
using Content;

namespace Ship
{
    namespace SecondEdition.TIEAdvancedV1
    {
        public class Inquisitor : TIEAdvancedV1
        {
            public Inquisitor() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Inquisitor",
                    3,
                    38,
                    force: 1,
                    extraUpgradeIcon: UpgradeType.ForcePower,
                    tags: new List<Tags>
                    {
                        Tags.DarkSide
                    },
                    seImageNumber: 102
                );
            }
        }
    }
}