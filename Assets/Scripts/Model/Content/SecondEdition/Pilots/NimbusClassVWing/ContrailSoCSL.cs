using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.NimbusClassVWing
    {
        public class ContrailSoCSL : NimbusClassVWing
        {
            public ContrailSoCSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Contrail\"",
                    5,
                    46,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.ContrailAbility),
                    tags: new List<Tags>
                    {
                        Tags.SoC,
                        Tags.SL
                    },
                    extraUpgradeIcon: UpgradeType.Talent,
                    isStandardLayout: true
                );
                ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

                MustHaveUpgrades.Add(typeof(IonLimiterOverride));
                MustHaveUpgrades.Add(typeof(Alpha3BBesh));
                MustHaveUpgrades.Add(typeof(IonBombs));
                MustHaveUpgrades.Add(typeof(PreciseAstromech));

                PilotNameCanonical = "contrail-siegeofcoruscant";
            }
        }
    }
}

