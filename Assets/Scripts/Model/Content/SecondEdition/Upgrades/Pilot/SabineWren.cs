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
            ImageUrl = "https://www.x-wing-cardcreator.com/img/published/Sabine%20Wren%20Pilot%20Ability_Sabine%20Wren%20Pilot%20Ability_0.png";
        }


    }
}