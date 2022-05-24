using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class EsegeTuketuPilotAbility : GenericUpgrade
    {
        public EsegeTuketuPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Esege Tuketu Pilot Ability",
                UpgradeType.Pilot,
                cost: 6,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        3
                    ),
                abilityType: typeof(Abilities.SecondEdition.EsegeTuketuAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/esegetuketu.png";
        }


    }
}