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
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Modification
                    },
                    tags: new List<Tags>
                    {
                        Tags.BoY,
                        Tags.SL
                    },
                    isStandardLayout: true
                );
                ShipInfo.Hull++;
                PilotNameCanonical = "maulermithel-battleofyavin";

                MustHaveUpgrades.Add(typeof(Predator));
                MustHaveUpgrades.Add(typeof(AfterBurners));
            }
        }
    }
}