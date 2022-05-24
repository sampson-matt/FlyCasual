using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class BodhiRookPilotAbility : GenericUpgrade
    {
        public BodhiRookPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Bodhi Rook Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        4
                    ),
                abilityType: typeof(Abilities.SecondEdition.BodhiRookAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/bodhirook.png";
        }


    }
}