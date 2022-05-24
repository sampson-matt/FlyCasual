using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class ArvelCrynydPilotAbility : GenericUpgrade
    {
        public ArvelCrynydPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Arvel Crynyd Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        3
                    ),
                abilityType: typeof(Abilities.SecondEdition.ArvelCrynydAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/arvelcrynyd.png";
        }


    }
}