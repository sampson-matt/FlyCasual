using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class DerekKlivianPilotAbility : GenericUpgrade
    {
        public DerekKlivianPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Derek Klivian Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                abilityType: typeof(Abilities.SecondEdition.DerekKlivianAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/derekklivian.png";
        }


    }
}