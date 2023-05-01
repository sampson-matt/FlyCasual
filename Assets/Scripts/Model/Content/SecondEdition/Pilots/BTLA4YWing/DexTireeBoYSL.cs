using Abilities.SecondEdition;
using BoardTools;
using Upgrade;
using Content;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.BTLA4YWing
    {
        public class DexTireeBoYSL : BTLA4YWing
        {
            public DexTireeBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Dex Tiree",
                    2,
                    38,
                    isLimited: true,
                    abilityType: typeof(DexTireeAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Turret,
                        UpgradeType.Torpedo,
                        UpgradeType.Astromech
                    },
                    isStandardLayout: true
                );
                ShipAbilities.Add(new HopeAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.DorsalTurret));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.AdvProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.R4Astromech));

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/f/ff/Dextiree-battleofyavin.png";
                PilotNameCanonical = "dextiree-battleofyavin-sl";
            }
        }
    }
}