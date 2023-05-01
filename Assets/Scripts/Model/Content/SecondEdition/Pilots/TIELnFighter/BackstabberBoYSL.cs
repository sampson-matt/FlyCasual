using Upgrade;
using Content;
using BoardTools;
using System.Collections.Generic;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class BackstabberBoYSL : TIELnFighter
        {
            public BackstabberBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Backstabber\"",
                    5,
                    38,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.BackstabberAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent,
                        UpgradeType.Modification
                    },
                    isStandardLayout: true
                );

                PilotNameCanonical = "backstabber-battleofyavin-sl";

                ShipInfo.Hull++;
                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/3/33/Backstabber-battleofyavin.png";

                MustHaveUpgrades.Add(typeof(CrackShot));
                MustHaveUpgrades.Add(typeof(Disciplined));
                MustHaveUpgrades.Add(typeof(AfterBurners));
            }
        }
    }
}