using BoardTools;
using Content;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship.SecondEdition.DroidTriFighter
{
    public class Dis347SoCSL : DroidTriFighter
    {
        public Dis347SoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "DIS-347",
                3,
                40,
                true,
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Modification },
                abilityType: typeof(Abilities.SecondEdition.Dis347Ability),
                tags: new List<Tags>
                {
                    Tags.Droid,
                    Tags.SoC,
                    Tags.SL
                },
                isStandardLayout: true
            );

            MustHaveUpgrades.Add(typeof(Marksmanship));
            MustHaveUpgrades.Add(typeof(AfterBurners));
            MustHaveUpgrades.Add(typeof(ContingencyProtocolSoC));

            PilotNameCanonical = "dis347-siegeofcoruscant";
        }
    }
}