using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class NetremPollardPilotAbility : GenericUpgrade
    {
        public NetremPollardPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Netrem Pollard Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                abilityType: typeof(Abilities.SecondEdition.NetremPollardAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/netrempollard.png";
        }


    }
}