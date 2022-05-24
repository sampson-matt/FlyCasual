using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class WedgeAntillesPilotAbility : GenericUpgrade
    {
        public WedgeAntillesPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Wedge Antilles Pilot Ability",
                UpgradeType.Pilot,

                cost: 12,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        6
                    ),
                abilityType: typeof(Abilities.FirstEdition.WedgeAntillesAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/wedgeantilles.png";
        }


    }
}