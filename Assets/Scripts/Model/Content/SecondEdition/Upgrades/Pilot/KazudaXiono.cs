using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class KazudaXionoPilotAbility : GenericUpgrade
    {
        public KazudaXionoPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Kazuda Xiono Pilot Ability",
                UpgradeType.Pilot,
                cost: 8,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.KazudaXionoAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Resistance/kazudaxiono.png";
        }


    }
}