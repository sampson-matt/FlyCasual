using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.TIESaBomber
    {
        public class Scimitar3BoE : TIESaBomber
        {
            public Scimitar3BoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Scimitar 3",
                    4,
                    57,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Abilities.SecondEdition.Scimitar3Ability),
                    extraUpgradeIcons: new List<UpgradeType>()
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent
                    }
                );

                MustHaveUpgrades.Add(typeof(NoEscape));
                MustHaveUpgrades.Add(typeof(PartingGift));
                MustHaveUpgrades.Add(typeof(ProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(ProtonBombs));

                PilotNameCanonical = "scimitar3-battleoverendor";
            }
        }
    }
}