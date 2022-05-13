using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class CaptainRexPilotAbility : GenericUpgrade
    {
        public CaptainRexPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Captain Rex Pilot Ability",
                UpgradeType.Pilot,

                cost: 4,
                abilityType: typeof(Abilities.SecondEdition.CaptainRexPilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/captainrex.png";
        }


    }
}