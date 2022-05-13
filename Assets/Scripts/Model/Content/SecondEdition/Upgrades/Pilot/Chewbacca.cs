using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class ChewbaccaPilotAbility : GenericUpgrade
    {
        public ChewbaccaPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Chewbacca Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.ChewbaccaRebelPilotAbility),
                charges: 1,
                regensCharges: true
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/chewbacca.png";
        }


    }
}