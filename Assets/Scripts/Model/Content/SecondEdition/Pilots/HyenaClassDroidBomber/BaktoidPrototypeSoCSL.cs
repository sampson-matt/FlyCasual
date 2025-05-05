using System;
using System.Collections.Generic;
using System.Linq;
using Content;
using Ship;
using Tokens;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship.SecondEdition.HyenaClassDroidBomber
{
    public class BaktoidPrototypeSoCSL : HyenaClassDroidBomber
    {
        public BaktoidPrototypeSoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "Baktoid Prototype",
                1,
                34,
                limited: 2,
                abilityType: typeof(Abilities.SecondEdition.BaktoidPrototypeAbility),
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Missile, UpgradeType.Configuration},
                pilotTitle: "Function over Form",
                tags: new List<Tags>
                {
                    Tags.Droid,
                    Tags.SoC,
                    Tags.SL
                },
                isStandardLayout: true
            );

            MustHaveUpgrades.Add(typeof(HomingMissiles));
            MustHaveUpgrades.Add(typeof(ContingencyProtocolSoC));
            MustHaveUpgrades.Add(typeof(StrutLockOverride));

            PilotNameCanonical = "baktoidprototype-siegeofcoruscant";
        }
    }
}