using ActionsList;
using Bombs;
using Content;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using UpgradesList.SecondEdition;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESaBomber
    {
        public class Scimitar1BoE : TIESaBomber
        {
            public Scimitar1BoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Scimitar 1",
                    3,
                    56,
                     tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Abilities.SecondEdition.Scimitar1Ability),
                    extraUpgradeIcons: new List<UpgradeType>()
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent
                    }
                );

                MustHaveUpgrades.Add(typeof(Marksmanship));
                MustHaveUpgrades.Add(typeof(NoEscapeBoE));
                MustHaveUpgrades.Add(typeof(ProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(IonBombs));

                PilotNameCanonical = "scimitar1-battleoverendor";
            }
        }
    }
}