using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class AP5PilotAbility : GenericUpgrade
    {
        public AP5PilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "AP-5 Pilot Ability",
                UpgradeType.Pilot,

                cost: 2,
                abilityType: typeof(Abilities.SecondEdition.AP5PilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/ap5.png";
        }


    }
}