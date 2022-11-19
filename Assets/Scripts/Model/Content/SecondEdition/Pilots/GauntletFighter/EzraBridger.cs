
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class EzraBridger : GauntletFighter
        {
            public EzraBridger() : base()
            {
                //IsWIP = true;

                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo
                (
                    "Ezra Bridger",
                    3,
                    65,
                    force: 1,
                    pilotTitle: "Spectre-6",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.EzraBridgerPilotAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.ForcePower},
                    factionOverride: Faction.Rebel
                );

                PilotNameCanonical = "ezrabridger-gauntletfighter";

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/3/3f/Ezra-gauntlet.png";
            }
        }
    }
}