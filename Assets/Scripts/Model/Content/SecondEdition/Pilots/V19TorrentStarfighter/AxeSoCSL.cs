using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship.SecondEdition.V19TorrentStarfighter
{
    public class AxeSoCSL : V19TorrentStarfighter
    {
        public AxeSoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "\"Axe\"",
                3,
                40,
                true,
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.SL
                },
                abilityType: typeof(Abilities.SecondEdition.AxeSoCAbility),
                extraUpgradeIcons: new List<UpgradeType>
                {
                    UpgradeType.Talent,
                    UpgradeType.Missile
                },
                isStandardLayout: true
            );
            ShipInfo.Hull++;
            ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
            ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

            MustHaveUpgrades.Add(typeof(DeadeyeShot));
            MustHaveUpgrades.Add(typeof(BarrageRockets));

            PilotNameCanonical = "axe-siegeofcoruscant";
        }
    }
}
