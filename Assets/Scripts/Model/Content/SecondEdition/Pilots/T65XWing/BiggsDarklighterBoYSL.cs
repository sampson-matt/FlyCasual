using Abilities.SecondEdition;
using System.Collections.Generic;
using Ship;
using SubPhases;
using Upgrade;
using Content;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class BiggsDarklighterBoYSL : T65XWing
        {
            public BiggsDarklighterBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Biggs Darklighter",
                    3,
                    56,
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent,
                        UpgradeType.Torpedo,
                        UpgradeType.Astromech
                    },
                    tags: new List<Tags>
                    {
                        Tags.BoY,
                        Tags.SL
                    },
                    abilityType: typeof(Abilities.SecondEdition.BiggsDarklighterBoYAbility),
                    isStandardLayout: true
                );
                ShipAbilities.Add(new HopeAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.AttackSpeed));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Selfless));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.ProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.R2F2BoY));

                PilotNameCanonical = "biggsdarklighter-battleofyavin";
                ModelInfo.SkinName = "Biggs Darklighter";
            }
        }
    }
}
