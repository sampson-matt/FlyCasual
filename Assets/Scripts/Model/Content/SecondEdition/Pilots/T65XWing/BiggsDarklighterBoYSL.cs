using Abilities.SecondEdition;
using System.Collections.Generic;
using Ship;
using SubPhases;
using BoardTools;
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
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    abilityType: typeof(Abilities.SecondEdition.BiggsDarklighterBoYAbility),
                    isStandardLayout: true
                );
                ShipAbilities.Add(new HopeAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.AttackSpeed));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Selfless));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.ProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.R2F2BoY));

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/8/8a/Biggsdarklighter-battleofyavin.png";
                PilotNameCanonical = "biggsdarklighter-battleofyavin-sl";
                ModelInfo.SkinName = "Biggs Darklighter";
            }
        }
    }
}
