using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship.SecondEdition.DroidTriFighter
{
    public class DisT81SoCSL : DroidTriFighter
    {
        public DisT81SoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "DIS-T81",
                4,
                48,
                true,
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Modification },
                abilityType: typeof(Abilities.SecondEdition.DisT81SoCAbility),
                pilotTitle: "Siege of Coruscant",
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.SL,
                    Tags.Droid
                },
                isStandardLayout: true
            );

            MustHaveUpgrades.Add(typeof(Outmaneuver));
            MustHaveUpgrades.Add(typeof(AfterBurners));
            MustHaveUpgrades.Add(typeof(ContingencyProtocolSoC));

            PilotNameCanonical = "dist81-siegeofcoruscant";
        }
    }
}
