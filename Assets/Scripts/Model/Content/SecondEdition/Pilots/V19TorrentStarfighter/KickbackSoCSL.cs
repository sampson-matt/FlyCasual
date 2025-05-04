using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship.SecondEdition.V19TorrentStarfighter
{
    public class KickbackSoCSL : V19TorrentStarfighter
    {
        public KickbackSoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "\"Kickback\"",
                5,
                41,
                true,
                abilityType: typeof(Abilities.SecondEdition.KickbackSoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.SL
                },
                extraUpgradeIcons: new List<UpgradeType>
                {
                    UpgradeType.Missile,
                    UpgradeType.Modification
                },
                isStandardLayout: true
            ); ;
            ShipInfo.Hull++;
            ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
            ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

            MustHaveUpgrades.Add(typeof(DiamondBoronMissiles));
            MustHaveUpgrades.Add(typeof(MunitionsFailsafe));

            PilotNameCanonical = "kickback-siegeofcoruscant";
        }
    }
}