using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class KullbeeSperadoPilotAbility : GenericUpgrade
    {
        public KullbeeSperadoPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Kullbee Sperado Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.FirstEdition.KullbeeSperadoAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/kullbeesperado.png";
        }


    }
}