using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class CassianAndorPilotAbility : GenericUpgrade
    {
        public CassianAndorPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Cassian Andor Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        3
                    ),
                abilityType: typeof(Abilities.SecondEdition.CassianAndorAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/cassianandor.png";
        }


    }
}