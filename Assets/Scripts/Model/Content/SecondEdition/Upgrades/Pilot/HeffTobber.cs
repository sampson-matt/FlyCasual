using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class HeffTobberPilotAbility : GenericUpgrade
    {
        public HeffTobberPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Heff Tobber Pilot Ability",
                UpgradeType.Pilot,

                cost: 4,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        2
                    ),
                abilityType: typeof(Abilities.SecondEdition.HeffTobberAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/hefftobber.png";
        }


    }
}