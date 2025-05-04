using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.NimbusClassVWing
    {
        public class KlickSoCSL : NimbusClassVWing
        {
            public KlickSoCSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Klick\"",
                    4,
                    41,
                    isLimited: true,
                    charges: 1,
                    regensCharges: 1,
                    abilityType: typeof(Abilities.SecondEdition.KlickAbility),
                    tags: new List<Tags>
                    {
                        Tags.SoC,
                        Tags.SL
                    },
                    extraUpgradeIcon: UpgradeType.Modification,
                    isStandardLayout: true
                );
                ShipInfo.Shields++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipInfo.Tags.Remove(Tags.Tie);
                ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

                MustHaveUpgrades.Add(typeof(R3Astromech));
                MustHaveUpgrades.Add(typeof(Alpha3EEsk));
                MustHaveUpgrades.Add(typeof(PrecisionIonEngines));

                PilotNameCanonical = "klick-siegeofcoruscant";
            }
        }
    }
}