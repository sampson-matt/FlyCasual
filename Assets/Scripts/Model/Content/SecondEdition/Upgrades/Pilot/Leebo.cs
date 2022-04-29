using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class LeeboPilotAbility : GenericUpgrade
    {
        public LeeboPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Leebo Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                abilityType: typeof(Abilities.SecondEdition.LeeboAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/leebo.png";
        }


    }
}