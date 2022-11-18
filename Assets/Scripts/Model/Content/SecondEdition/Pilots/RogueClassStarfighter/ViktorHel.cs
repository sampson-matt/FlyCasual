using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class ViktorHel : RogueClassStarfighter
        {
            public ViktorHel() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Viktor Hel",
                    4,
                    39,
                    pilotTitle: "Storied Bounty Hunter",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.ViktorHelAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );

                PilotNameCanonical = "viktorhel-rogueclassstarfighter";

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/viktorhel-rogueclassstarfighter.png";
            }
        }
    }
}