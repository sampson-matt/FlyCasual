using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship.SecondEdition.VultureClassDroidFighter
{
    public class Dfs081SoCSL : VultureClassDroidFighter
    {
        public Dfs081SoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "DFS-081",
                3,
                28,
                true,
                charges: 2,
                abilityType: typeof(Abilities.SecondEdition.Dfs081SoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.SL,
                    Tags.Droid
                },
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Missile, UpgradeType.Configuration },
                pilotTitle: "Siege of Coruscant",
                isStandardLayout: true
            );

            MustHaveUpgrades.Add(typeof(DiscordMissiles));
             MustHaveUpgrades.Add(typeof(ContingencyProtocolSoC));
            MustHaveUpgrades.Add(typeof(StrutLockOverride));

            PilotNameCanonical = "dfs081-siegeofcoruscant";
        }
    }
}