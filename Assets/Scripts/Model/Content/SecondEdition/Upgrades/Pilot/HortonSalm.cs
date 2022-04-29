using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class HortonSalmPilotAbility : GenericUpgrade
    {
        public HortonSalmPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Horton Salm Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.HortonSalmAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/hortonsalm.png";
        }


    }
}