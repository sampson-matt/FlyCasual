using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using Content;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ARC170Starfighter
    {
        public class WolffeSoCSL : ARC170Starfighter
        {
            public WolffeSoCSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Wolffe\"",
                    4,
                    56,
                    isLimited: true,
                    factionOverride: Faction.Republic,
                    abilityType: typeof(Abilities.SecondEdition.WolffeAbility),
                    tags: new List<Tags>
                    {
                        Tags.SoC,
                        Tags.SL
                    },
                    extraUpgradeIcon: UpgradeType.Gunner,
                    charges: 1,
                    isStandardLayout: true
                );
                ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

                PilotNameCanonical = "wolffe-siegeofcoruscant";

                ModelInfo.SkinName = "Wolffe";

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.WolfpackSoC));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.VeteranTailGunner));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Q7Astromech));
            }
        }
    }
}