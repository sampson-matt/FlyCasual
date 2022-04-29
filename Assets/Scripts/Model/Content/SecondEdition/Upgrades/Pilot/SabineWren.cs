using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class SabineWrenPilotAbility : GenericUpgrade
    {
        public SabineWrenPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Sabine Wren Pilot Ability",
                UpgradeType.Pilot,
                cost: 8,
                abilityType: typeof(Abilities.FirstEdition.SabineWrenPilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/sabinewren.png";
        }


    }
}