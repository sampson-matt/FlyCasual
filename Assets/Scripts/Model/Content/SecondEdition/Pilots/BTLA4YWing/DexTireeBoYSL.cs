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
                        Tags.BoY,
                        Tags.SL
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

                PilotNameCanonical = "dextiree-battleofyavin";
            }
        }
    }
}