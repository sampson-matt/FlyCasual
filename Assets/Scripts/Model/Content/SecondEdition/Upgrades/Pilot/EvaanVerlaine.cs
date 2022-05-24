using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class EvaanVerlainePilotAbility : GenericUpgrade
    {
        public EvaanVerlainePilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Evaan Verlaine Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        3
                    ),
                abilityType: typeof(Abilities.SecondEdition.EvaanVerlaineAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/evaanverlaine.png";
        }


    }
}