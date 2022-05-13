using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class RoarkGarnetPilotAbility : GenericUpgrade
    {
        public RoarkGarnetPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Roark Garnet Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.RoarkGarnetAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/roarkgarnet.png";
        }


    }
}