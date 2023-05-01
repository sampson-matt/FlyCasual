using Abilities.SecondEdition;
using Arcs;
using Ship;
using SubPhases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Content;
using UnityEngine;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTLA4YWing
    {
        public class PopsKrailBoYSL : BTLA4YWing
        {
            public PopsKrailBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Pops\" Krail",
                    3,
                    45,
                    pilotTitle: "Battle of Yavin",
                    isLimited: true,
                    abilityType: typeof(PopsKrailBoYAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Turret,
                        UpgradeType.Torpedo,
                        UpgradeType.Astromech,
                        UpgradeType.Device,
                        UpgradeType.Missile,
                        UpgradeType.Modification
                    },
                    isStandardLayout: true
                );
                ShipAbilities.Add(new HopeAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.IonCannonTurret));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.AdvProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.R4Astromech));

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/9/9b/Popskrail-battleofyavin.png";
                PilotNameCanonical = "popskrail-battleofyavin-sl";
            }
        }
    }
}