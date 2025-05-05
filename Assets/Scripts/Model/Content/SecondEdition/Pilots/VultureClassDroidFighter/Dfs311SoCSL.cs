using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship.SecondEdition.VultureClassDroidFighter
{
    public class Dfs311SoCSL : VultureClassDroidFighter
    {
        public Dfs311SoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "DFS-311",
                1,
                28,
                true,
                abilityType: typeof(Abilities.SecondEdition.Dfs311Ability),
                pilotTitle: "Siege of Coruscant",
                tags: new List<Tags>
                {
                    Tags.Droid,
                    Tags.SoC,
                    Tags.SL
                },
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Missile, UpgradeType.Configuration },
                isStandardLayout: true
            );

            MustHaveUpgrades.Add(typeof(DiscordMissiles));
            MustHaveUpgrades.Add(typeof(ContingencyProtocolSoC));
            MustHaveUpgrades.Add(typeof(StrutLockOverride));

            PilotNameCanonical = "dfs311-siegeofcoruscant";
        }
    }
}