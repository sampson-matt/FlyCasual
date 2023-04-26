using Upgrade;
using Content;
using BoardTools;
using System.Collections.Generic;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class MaulerMithelBoYSL : TIELnFighter
        {
            public MaulerMithelBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Mauler\" Mithel",
                    5,
                    37,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.MaulerMithelBoYAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcon: UpgradeType.Talent,
                    isStandardLayout: true
                );
                ShipInfo.Hull++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                PilotNameCanonical = "maulermithel-battleofyavin-sl";
                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/4/41/Maulermithel-battleofyavin.png";

                MustHaveUpgrades.Add(typeof(Predator));
                MustHaveUpgrades.Add(typeof(AfterBurners));
            }
        }
    }
}