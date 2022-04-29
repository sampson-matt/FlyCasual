using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class HeffTobberPilotAbility : GenericUpgrade
    {
        public HeffTobberPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Heff Tobber Pilot Ability",
                UpgradeType.Pilot,

                cost: 4,
                abilityType: typeof(Abilities.SecondEdition.HeffTobberAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/hefftobber.png";
        }


    }
}