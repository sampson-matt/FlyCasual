using Abilities.SecondEdition;
using System;
using Ship;
using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.TIEAdvancedX1
    {
        public class DarthVaderBOYSL : TIEAdvancedX1
        {
            public DarthVaderBOYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Darth Vader",
                    6,
                    81,
                    isLimited: true,
                    abilityType: typeof(DarthVaderBoYAbility),
                    force: 3,
                    extraUpgradeIcons: new List<UpgradeType>()
                    {
                        UpgradeType.ForcePower,
                        UpgradeType.Talent,
                        UpgradeType.Modification
                    },
                    tags: new List<Tags>
                    {
                        Tags.BoY,
                        Tags.DarkSide,
                        Tags.Sith
                    },
                    isStandardLayout: true
                );
                ShipInfo.Shields++;
                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/a/a9/Darthvader-battleofyavin.png";
                PilotNameCanonical = "darthvader-battleofyavin-sl";
                ModelInfo.SkinName = "Blue";

                MustHaveUpgrades.Add(typeof(Marksmanship));
                MustHaveUpgrades.Add(typeof(Hate));
                MustHaveUpgrades.Add(typeof(AfterBurners));
            }
        }
    }
}