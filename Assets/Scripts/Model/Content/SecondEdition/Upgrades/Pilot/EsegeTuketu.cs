using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class EsegeTuketuPilotAbility : GenericUpgrade
    {
        public EsegeTuketuPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Esege Tuketu Pilot Ability",
                UpgradeType.Pilot,
                cost: 8,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.EsegeTuketuAbility)
            );
            ImageUrl = "https://www.x-wing-cardcreator.com/img/published/Esege%20Tuketu%20Pilot%20Ability_Esege%20Tuketu%20Pilot%20Ability_0.png";
        }


    }
}