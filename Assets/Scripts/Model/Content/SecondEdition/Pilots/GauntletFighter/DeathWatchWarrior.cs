using Content;
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
                    53,
                    pilotTitle: "Fanatical Adherent",
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.Mandalorian
                    },
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Illicit },
                    factionOverride: Faction.Separatists
                );

                ModelInfo.SkinName = "CIS Dark";
            }
        }
    }
}