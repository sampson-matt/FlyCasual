using BoardTools;
using Ship;
using SubPhases;
using Content;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.NimbusClassVWing
    {
        public class KlickSoC : NimbusClassVWing
        {
            public KlickSoC() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Klick\"",
                    4,
                    36,
                    isLimited: true,
                    charges: 1,
                    regensCharges: 1,
                    abilityType: typeof(Abilities.SecondEdition.KlickAbility),
                    tags: new List<Tags>
                    {
                        Tags.SoC
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ShipInfo.Shields++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipInfo.Tags.Remove(Tags.Tie);
                ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

                PilotNameCanonical = "klick-siegeofcoruscant-lsl";
            }
        }
    }
}