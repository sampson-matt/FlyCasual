using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class AirenCrackenPilotAbility : GenericUpgrade
    {
        public AirenCrackenPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Airen Cracken Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                abilityType: typeof(Abilities.SecondEdition.AirenCrackenAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/airencracken.png";
        }


    }
}