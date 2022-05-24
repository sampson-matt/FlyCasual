using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class GarvenDreisPilotAbility : GenericUpgrade
    {
        public GarvenDreisPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Garven Dreis Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        4
                    ),
                abilityType: typeof(Abilities.SecondEdition.GarvenDreisAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/garvendreis.png";
        }


    }
}