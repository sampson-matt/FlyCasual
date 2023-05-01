using ActionsList;
using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;
using Content;
using Abilities.SecondEdition;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class LukeSkywalkerBoYSL : T65XWing
        {
            public LukeSkywalkerBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Luke Skywalker",
                    5,
                    79,
                    isLimited: true,
                    abilityType: typeof(LukeSkywalkerAbility),
                    force: 2,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.ForcePower,
                        UpgradeType.Talent,
                        UpgradeType.Torpedo,
                        UpgradeType.Astromech
                    },
                    tags: new List<Tags>
                    {
                        Tags.BoY,
                        Tags.LightSide
                    },
                    isStandardLayout: true
                );
                ShipAbilities.Add(new HopeAbility());

                MustHaveUpgrades.Add(typeof(AttackSpeed));
                MustHaveUpgrades.Add(typeof(InstinctiveAim));
                MustHaveUpgrades.Add(typeof(ProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(R2D2BoY));

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/6/61/Lukeskywalker-battleofyavin.png";
                PilotNameCanonical = "lukeskywalker-battleofyavin-sl";
                ModelInfo.SkinName = "Luke Skywalker";
            }
        }
    }
}