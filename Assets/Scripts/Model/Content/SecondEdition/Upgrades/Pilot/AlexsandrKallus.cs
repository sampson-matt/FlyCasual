using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class AlexsandrKallusPilotAbility : GenericUpgrade
    {
        public AlexsandrKallusPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Alexsandr Kallus Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        4
                    ),
                abilityType: typeof(Abilities.SecondEdition.AlexandrKallusAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/alexsandrkallus.png";
        }


    }
}