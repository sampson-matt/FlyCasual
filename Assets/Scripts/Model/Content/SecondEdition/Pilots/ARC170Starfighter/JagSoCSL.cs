using Ship;
using System;
using System.Collections.Generic;
using Content;

namespace Ship
{
    namespace SecondEdition.ARC170Starfighter
    {
        public class JagSoCSL : ARC170Starfighter
        {
            public JagSoCSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Jag\"",
                    3,
                    51,
                    isLimited: true,
                    factionOverride: Faction.Republic,
                    abilityType: typeof(Abilities.SecondEdition.JagSoCAbility),
                    tags: new List<Tags>
                    {
                        Tags.SoC,
                        Tags.SL
                    },
                    isStandardLayout: true
                );
                ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

                PilotNameCanonical = "jag-siegeofcoruscant";

                ModelInfo.SkinName = "Red";

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.VeteranTailGunner));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.SynchronizedConsole));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.R4PAstromech));
            }
        }
    }
}