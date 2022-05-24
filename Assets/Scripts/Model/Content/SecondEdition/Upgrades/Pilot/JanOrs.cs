using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class JanOrsPilotAbility : GenericUpgrade
    {
        public JanOrsPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Jan Ors Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        5
                    ),
                abilityType: typeof(Abilities.SecondEdition.JanOrsAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/janors.png";
        }


    }
}