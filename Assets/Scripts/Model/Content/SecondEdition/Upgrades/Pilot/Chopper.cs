using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class ChopperPilotAbility : GenericUpgrade
    {
        public ChopperPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Chopper Pilot Ability",
                UpgradeType.Pilot,

                cost: 4,
                abilityType: typeof(Abilities.SecondEdition.ChopperPilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/chopper.png";
        }


    }
}