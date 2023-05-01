using Abilities.SecondEdition;
using System.Collections.Generic;
using Content;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class JekPorkinsBoYSL : T65XWing
        {
            public JekPorkinsBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Jek Porkins",
                    4,
                    54,
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Torpedo,
                        UpgradeType.Astromech,
                        UpgradeType.Modification
                    },
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    abilityType: typeof(JekPorkinsAbility),
                    isStandardLayout: true
                );
                ShipAbilities.Add(new HopeAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.AdvProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.R5D8BoY));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.UnstableSublightEngines));

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/1/1b/Jekporkins-battleofyavin.png";
                PilotNameCanonical = "jekporkins-battleofyavin-sl";
                ModelInfo.SkinName = "Jek Porkins";
            }
        }
    }
}