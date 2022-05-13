using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class CorranHornPilotAbility : GenericUpgrade
    {
        public CorranHornPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Corran Horn Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                abilityType: typeof(Abilities.SecondEdition.CorranHornAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/corranhorn.png";
        }


    }
}