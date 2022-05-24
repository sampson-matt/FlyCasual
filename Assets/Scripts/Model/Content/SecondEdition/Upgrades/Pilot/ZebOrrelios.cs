using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class ZebOrreliosPilotAbility : GenericUpgrade
    {
        public ZebOrreliosPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Zeb Orrelios Pilot Ability",
                UpgradeType.Pilot,

                cost: 4,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        2
                    ),
                abilityType: typeof(Abilities.FirstEdition.ZebOrreliosPilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/zeborrelios.png";
        }


    }
}