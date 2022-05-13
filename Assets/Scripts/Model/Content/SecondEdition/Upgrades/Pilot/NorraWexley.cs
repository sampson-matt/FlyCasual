using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class NorraWexleyPilotAbility : GenericUpgrade
    {
        public NorraWexleyPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Norra Wexley Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                abilityType: typeof(Abilities.SecondEdition.NorraWexleyAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/norrawexley.png";
        }


    }
}