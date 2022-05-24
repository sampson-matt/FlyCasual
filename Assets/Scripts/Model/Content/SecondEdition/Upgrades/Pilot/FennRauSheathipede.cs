using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class FennRauSheathipedePilotAbility : GenericUpgrade
    {
        public FennRauSheathipedePilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Fenn Rau Sheathipede Pilot Ability",
                UpgradeType.Pilot,

                cost: 12,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        6
                    ),
                abilityType: typeof(Abilities.FirstEdition.FennRauRebelAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/fennrausheathipede.png";
        }


    }
}