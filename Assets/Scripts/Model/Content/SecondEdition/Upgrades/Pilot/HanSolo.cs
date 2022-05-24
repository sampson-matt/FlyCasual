using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class HanSoloPilotAbility : GenericUpgrade
    {
        public HanSoloPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Han Solo Pilot Ability",
                UpgradeType.Pilot,

                cost: 12,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        6
                    ),
                abilityType: typeof(Abilities.SecondEdition.HanSoloRebelPilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/hansolo.png";
        }


    }
}