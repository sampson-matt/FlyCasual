using ActionsList;
using Arcs;
using Content;
using UpgradesList.SecondEdition;
using Upgrade;
using System.Collections.Generic;

namespace Ship.SecondEdition.VultureClassDroidFighter
{
    public class HaorChallPrototypeSoCSL : VultureClassDroidFighter
    {
        public HaorChallPrototypeSoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "Haor Chall Prototype",
                1,
                25,
                limited: 2,
                abilityType: typeof(Abilities.SecondEdition.HaorChallPrototypeAbility),
                pilotTitle: "Siege of Coruscant",
                tags: new List<Tags>
                {
                    Tags.Droid,
                    Tags.SoC,
                    Tags.SL
                },
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Missile, UpgradeType.Configuration },
                isStandardLayout: true
            );

            MustHaveUpgrades.Add(typeof(IonMissiles));
            MustHaveUpgrades.Add(typeof(ContingencyProtocolSoC));
            MustHaveUpgrades.Add(typeof(StrutLockOverride));

            PilotNameCanonical = "haorchallprototype-siegeofcoruscant";

            ModelInfo.SkinName = "Gray";
        }
    }
}