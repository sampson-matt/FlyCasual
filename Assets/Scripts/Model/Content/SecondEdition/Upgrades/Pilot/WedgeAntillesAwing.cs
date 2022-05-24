using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class WedgeAntillesAwingPilotAbility : GenericUpgrade
    {
        public WedgeAntillesAwingPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Wedge Antilles Awing Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        4
                    ),
                abilityType: typeof(Abilities.SecondEdition.WedgeAntillesAWingAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/wedgeantillesawing.png";
        }


    }
}