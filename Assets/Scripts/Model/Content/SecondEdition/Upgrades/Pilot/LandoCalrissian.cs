using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class LandoCalrissianPilotAbility : GenericUpgrade
    {
        public LandoCalrissianPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Lando Calrissian Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        5
                    ),
                abilityType: typeof(Abilities.SecondEdition.LandoCalrissianRebelPilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/landocalrissian.png";
        }


    }
}