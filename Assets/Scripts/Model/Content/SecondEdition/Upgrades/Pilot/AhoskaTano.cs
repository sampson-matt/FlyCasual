using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class AhsokaTanoPilotAbility : GenericUpgrade
    {
        public AhsokaTanoPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Ahsoka Tano Pilot Ability",
                UpgradeType.Pilot,

                cost: 30,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        5
                    ),
                abilityType: typeof(Abilities.SecondEdition.AhsokaTanoRebelAbility),
                addForce: 3
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/ahsokatano.png";
        }


    }
}