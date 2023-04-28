using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ARC170Starfighter
    {
        public class P104thBattalionPilot : ARC170Starfighter
        {
            public P104thBattalionPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "104th Battalion Pilot",
                    2,
                    42,
                    factionOverride: Faction.Republic
                );

                ModelInfo.SkinName = "Red";
            }
        }
    }
}
