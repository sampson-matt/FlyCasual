using Abilities.FirstEdition;
using System.Collections.Generic;
using Content;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class GarvenDreisBoYSL : T65XWing
        {
            public GarvenDreisBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Garven Dreis",
                    4,
                    53,
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
                    abilityType: typeof(GarvenDreisAbility),
                    isStandardLayout: true
                );
                ShipAbilities.Add(new Abilities.SecondEdition.HopeAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.AdvProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.R5K6BoY));

                PilotNameCanonical = "garvendreis-battleofyavin";
            }
        }
    }
}