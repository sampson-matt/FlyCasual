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
                        Tags.BoY,
                        Tags.SL
                    },
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent,
                        UpgradeType.Modification
                    },
                    isStandardLayout: true
                );

                PilotNameCanonical = "backstabber-battleofyavin";

                ShipInfo.Hull++;

                MustHaveUpgrades.Add(typeof(CrackShot));
                MustHaveUpgrades.Add(typeof(Disciplined));
                MustHaveUpgrades.Add(typeof(AfterBurners));
            }
        }
    }
}