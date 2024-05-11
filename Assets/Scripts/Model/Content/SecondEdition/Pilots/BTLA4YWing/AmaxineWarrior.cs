using System;
using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTLA4YWing
    {
        public class AmaxineWarrior : BTLA4YWing
        {
            public AmaxineWarrior() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Amaxine Warrior",
                    3,
                    31,
                    extraUpgradeIcons: new List<UpgradeType>()
                    {
                        UpgradeType.Talent,
                        UpgradeType.Tech
                    },
                    factionOverride: Faction.Scum
                );
            }
        }
    }
}
