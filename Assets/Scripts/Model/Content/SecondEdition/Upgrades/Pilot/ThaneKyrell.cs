using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class ThaneKyrellPilotAbility : GenericUpgrade
    {
        public ThaneKyrellPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Thane Kyrell Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                abilityType: typeof(Abilities.SecondEdition.ThaneKyrellAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/thanekyrell.png";
        }


    }
}