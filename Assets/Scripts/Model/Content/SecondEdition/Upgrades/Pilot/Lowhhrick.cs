using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class LowhhrickPilotAbility : GenericUpgrade
    {
        public LowhhrickPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Lowhhrick Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                abilityType: typeof(Abilities.FirstEdition.LowhhrickAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/lowhhrick.png";
        }


    }
}