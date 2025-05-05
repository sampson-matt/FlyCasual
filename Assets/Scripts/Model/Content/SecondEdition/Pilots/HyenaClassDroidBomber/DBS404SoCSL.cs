using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship.SecondEdition.HyenaClassDroidBomber
{
    public class DBS404SoCSL : HyenaClassDroidBomber
    {
        public DBS404SoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "DBS-404",
                4,
                36,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.DBS404SoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.SL,
                    Tags.Droid
                },
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Torpedo, UpgradeType.Configuration},
                pilotTitle: "Siege of Coruscant",
                isStandardLayout: true
            );

            MustHaveUpgrades.Add(typeof(AdvProtonTorpedoes));
            MustHaveUpgrades.Add(typeof(ContingencyProtocolSoC));
            MustHaveUpgrades.Add(typeof(StrutLockOverride));

            PilotNameCanonical = "dbs404-siegeofcoruscant";
        }
    }
}