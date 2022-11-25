using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class DeathWatchWarrior : GauntletFighter
        {
            public DeathWatchWarrior() : base()
            {

                PilotInfo = new PilotCardInfo
                (
                    "Death Watch Warrior",
                    2,
                    52,
                    pilotTitle: "Fanatical Adherent",
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Illicit },
                    factionOverride: Faction.Separatists
                );

                ModelInfo.SkinName = "CIS Dark";

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/e/ed/Deathwatchwarrior.png";
            }
        }
    }
}