using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship.SecondEdition.DroidTriFighter
{
    public class PhlacArphoccPrototypeSoCSL : DroidTriFighter
    {
        public PhlacArphoccPrototypeSoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "Phlac-Arphocc Prototype",
                5,
                50,
                limited: 2,
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Modification, UpgradeType.Modification },
                abilityType: typeof(Abilities.SecondEdition.PhlacArphoccPrototypeSoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.SL,
                    Tags.Droid
                },
                pilotTitle: "Siege of Coruscant",
                isStandardLayout: true
            );

            MustHaveUpgrades.Add(typeof(AfterBurners));
            MustHaveUpgrades.Add(typeof(ContingencyProtocolSoC));
            MustHaveUpgrades.Add(typeof(EvasionSequence7));

            PilotNameCanonical = "phlacarphoccprototype-siegeofcoruscant";
        }
    }
}