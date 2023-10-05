using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.FiresprayClassPatrolCraft
    {
        public class SeparatistRacketeer : FiresprayClassPatrolCraft
        {
            public SeparatistRacketeer() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Separatist Racketeer",
                    2,
                    62,
                    factionOverride: Faction.Separatists,
                    extraUpgradeIcon: UpgradeType.Crew
                );

                ModelInfo.SkinName = "Jango Fett";
            }
        }
    }
}
