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
                abilityType: typeof(Abilities.SecondEdition.EsegeTuketuAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/esegetuketu.png";
        }


    }
}