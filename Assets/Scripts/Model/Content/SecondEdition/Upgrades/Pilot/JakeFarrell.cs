using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class JakeFarrellPilotAbility : GenericUpgrade
    {
        public JakeFarrellPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Jake Farrell Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        4
                    ),
                abilityType: typeof(Abilities.SecondEdition.JakeFarrellAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/jakefarrell.png";
        }


    }
}