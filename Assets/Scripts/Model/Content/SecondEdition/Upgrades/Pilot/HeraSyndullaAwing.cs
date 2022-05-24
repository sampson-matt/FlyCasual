using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class HeraSyndullaAwingPilotAbility : GenericUpgrade
    {
        public HeraSyndullaAwingPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Hera Syndulla Awing Pilot Ability",
                UpgradeType.Pilot,

                cost: 12,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        6
                    ),
                abilityType: typeof(Abilities.SecondEdition.HeraSyndullaABWingAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/herasyndullaawing.png";
        }


    }
}