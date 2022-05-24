using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class FennRauPilotAbility : GenericUpgrade
    {
        public FennRauPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Fenn Rau Pilot Ability",
                UpgradeType.Pilot,

                cost: 12,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        6
                    ),
                abilityType: typeof(Abilities.SecondEdition.FennRauRebelFangAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/fennrau.png";
        }


    }
}