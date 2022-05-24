using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class GinaMoonsongPilotAbility : GenericUpgrade
    {
        public GinaMoonsongPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Gina Moonsong Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        5
                    ),
                abilityType: typeof(Abilities.SecondEdition.GinaMoonsongAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/ginamoonsong.png";
        }


    }
}