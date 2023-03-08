
using Content;
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

                PilotInfo = new PilotCardInfo
                (
                    "Ezra Bridger",
                    3,
                    65,
                    force: 1,
                    pilotTitle: "Spectre-6",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.EzraBridgerPilotAbility),
                    tags: new List<Tags>
                    {
                        Tags.LightSide,
                        Tags.Spectre
                    },
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.ForcePower},
                    factionOverride: Faction.Rebel
                );

                ModelInfo.SkinName = "Red";

                PilotNameCanonical = "ezrabridger-gauntletfighter";

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/3/3f/Ezra-gauntlet.png";
            }
        }
    }
}