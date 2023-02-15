using Ship;
using Content;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.NimbusClassVWing
    {
        public class ContrailSoC : NimbusClassVWing
        {
            public ContrailSoC() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Contrail\"",
                    5,
                    33,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.ContrailAbility),
                    tags: new List<Tags>
                    {
                        Tags.SoC
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

                ShipInfo.Tags.Remove(Tags.Tie);

                PilotNameCanonical = "contrail-siegeofcoruscant";

                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/contrail-soc.png";
            }
        }
    }
}

