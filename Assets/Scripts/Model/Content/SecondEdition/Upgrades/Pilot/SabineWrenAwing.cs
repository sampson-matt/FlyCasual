using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class SabineWrenAwingPilotAbility : GenericUpgrade
    {
        public SabineWrenAwingPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Sabine Wren Awing Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        3
                    ),
                abilityType: typeof(Abilities.SecondEdition.SabineWrenAWingAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/sabinewrenawing.png";
        }


    }
}