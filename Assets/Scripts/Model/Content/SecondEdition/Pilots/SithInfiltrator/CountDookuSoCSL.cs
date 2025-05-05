using System;
using System.Collections.Generic;
using Ship;
using Upgrade;
using BoardTools;
using SubPhases;
using Tokens;
using System.Linq;
using Content;
using UpgradesList.SecondEdition;

namespace Ship.SecondEdition.SithInfiltrator
{
    public class CountDookuSoCSL : SithInfiltrator
    {
        public CountDookuSoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "Count Dooku",
                5,
                75,
                true,
                abilityType: typeof(Abilities.SecondEdition.CountDookuCrewAbility),
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.SL,
                    Tags.DarkSide,
                    Tags.Sith
                },
                pilotTitle: "Siege of Coruscant",
                force: 3,
                extraUpgradeIcon: UpgradeType.ForcePower,
                isStandardLayout: true
            );

            MustHaveUpgrades.Add(typeof(Malice));
            MustHaveUpgrades.Add(typeof(RoilingAngerSoC));
            MustHaveUpgrades.Add(typeof(Scimitar));

            PilotNameCanonical = "countdooku-siegeofcoruscant";
        }
    }
}