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
                        Tags.BoY,
                        Tags.SL
                    },
                    abilityType: typeof(JekPorkinsAbility),
                    isStandardLayout: true
                );
                ShipAbilities.Add(new HopeAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.AdvProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.R5D8BoY));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.UnstableSublightEngines));

                PilotNameCanonical = "jekporkins-battleofyavin";
                ModelInfo.SkinName = "Jek Porkins";
            }
        }
    }
}