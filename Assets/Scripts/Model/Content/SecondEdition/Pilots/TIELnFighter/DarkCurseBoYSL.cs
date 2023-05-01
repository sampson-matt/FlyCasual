using Upgrade;
using Ship;
using Content;
using System.Collections.Generic;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class DarkCurseBoYSL : TIELnFighter
        {
            public DarkCurseBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Dark Curse\"",
                    6,
                    37,
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Modification
                    },
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    abilityType: typeof(Abilities.SecondEdition.DarkCurseAbility),
                    isStandardLayout:true
                );

                PilotNameCanonical = "darkcurse-battleofyavin-SL";

                ShipInfo.Hull++;

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/9/90/Darkcurse-battleofyavin.png";

                MustHaveUpgrades.Add(typeof(Ruthless));
                MustHaveUpgrades.Add(typeof(PrecisionIonEngines));
            }
        }
    }
}