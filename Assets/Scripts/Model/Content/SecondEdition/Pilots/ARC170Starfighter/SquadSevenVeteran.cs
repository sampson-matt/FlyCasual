using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ARC170Starfighter
    {
        public class SquadSevenVeteran : ARC170Starfighter
        {
            public SquadSevenVeteran() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Squad Seven Veteran",
                    3,
                    44,
                    factionOverride: Faction.Republic,
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ModelInfo.SkinName = "Red";
            }
        }
    }
}
