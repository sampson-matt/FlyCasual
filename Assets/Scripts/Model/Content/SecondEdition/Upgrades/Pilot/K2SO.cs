using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class K2SOPilotAbility : GenericUpgrade
    {
        public K2SOPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "K2SO Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                abilityType: typeof(Abilities.SecondEdition.K2SOPilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/k2so.png";
        }


    }
}