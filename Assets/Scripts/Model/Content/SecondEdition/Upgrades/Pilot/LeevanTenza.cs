using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class LeevanTenzaPilotAbility : GenericUpgrade
    {
        public LeevanTenzaPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Leevan Tenza Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        3
                    ),
                abilityType: typeof(Abilities.SecondEdition.LeevanTenzaAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/leevantenza.png";
        }


    }
}