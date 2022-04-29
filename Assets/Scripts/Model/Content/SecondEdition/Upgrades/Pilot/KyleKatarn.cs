using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class KyleKatarnPilotAbility : GenericUpgrade
    {
        public KyleKatarnPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Kyle Katarn Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                abilityType: typeof(Abilities.SecondEdition.KyleKatarnAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/kylekatarn.png";
        }


    }
}