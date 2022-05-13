using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class TenNumbPilotAbility : GenericUpgrade
    {
        public TenNumbPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Ten Numb Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.TenNumbAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/tennumb.png";
        }


    }
}