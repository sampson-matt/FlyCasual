using Abilities.SecondEdition;
using ActionsList;
using Ship;
using SubPhases;
using System;
using System.Collections;
using System.Collections.Generic;
using Content;
using Tokens;
using UnityEngine;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTLA4YWing
    {
        public class DutchVanderBoYSL : BTLA4YWing
        {
            public DutchVanderBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Dutch\" Vander",
                    4,
                    61,
                    isLimited: true,
                    abilityType: typeof(DutchVanderBoYAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Turret,
                        UpgradeType.Torpedo,
                        UpgradeType.Astromech
                    },
                    isStandardLayout: true
                );
                ShipAbilities.Add(new HopeAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.IonCannonTurret));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.AdvProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.TargetingAstromech));

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/d/d4/Dutchvander-battleofyavin.png";
                PilotNameCanonical = "dutchvander-battleofyavin-sl";
            }
        }
    }
}