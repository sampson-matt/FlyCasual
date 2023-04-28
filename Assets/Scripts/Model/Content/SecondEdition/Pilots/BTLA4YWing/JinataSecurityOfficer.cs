using System;
using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTLA4YWing
    {
        public class JinataSecurityOfficer : BTLA4YWing
        {
            public JinataSecurityOfficer() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Jinata Security Officer",
                    2,
                    30,
                    extraUpgradeIcon: UpgradeType.Tech,
                    factionOverride: Faction.Scum
                );
            }
        }
    }
}
