using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class DashRendarPilotAbility : GenericUpgrade
    {
        public DashRendarPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Dash Rendar Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        5
                    ),
                abilityType: typeof(Abilities.SecondEdition.DashRendarAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/dashrendar.png";
        }


    }
}