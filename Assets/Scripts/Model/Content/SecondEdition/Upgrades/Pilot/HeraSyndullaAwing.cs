using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class HeraSyndullaAwingPilotAbility : GenericUpgrade
    {
        public HeraSyndullaAwingPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Hera Syndulla Awing Pilot Ability",
                UpgradeType.Pilot,

                cost: 12,
                abilityType: typeof(Abilities.SecondEdition.HeraSyndullaABWingAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/herasyndullaawing.png";
        }


    }
}