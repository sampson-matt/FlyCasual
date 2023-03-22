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
        public class WolffeSoC : ARC170Starfighter
        {
            public WolffeSoC() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Wolffe\"",
                    4,
                    50,
                    isLimited: true,
                    factionOverride: Faction.Republic,
                    abilityType: typeof(Abilities.SecondEdition.WolffeAbility),
                    tags: new List<Tags>
                    {
                        Tags.SoC
                    },
                    extraUpgradeIcon: UpgradeType.Talent,
                    charges: 1
                );
                ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

                PilotNameCanonical = "wolffe-siegeofcoruscant";

                ModelInfo.SkinName = "Wolffe";

                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/wolffe-soc.png";
            }
        }
    }
}