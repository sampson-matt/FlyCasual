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
                //IsWIP = true;

                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

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

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/e/ed/Deathwatchwarrior.png";
            }
        }
    }
}