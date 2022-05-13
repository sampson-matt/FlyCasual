using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class MagvaYarroPilotAbility : GenericUpgrade
    {
        public MagvaYarroPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Magva Yarro Pilot Ability",
                UpgradeType.Pilot,

                cost: 3,
                abilityType: typeof(Abilities.SecondEdition.MagvaYarroPilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/magvayarro.png";
        }


    }
}