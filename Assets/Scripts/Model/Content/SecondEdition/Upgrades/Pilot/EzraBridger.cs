using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class EzraBridgerPilotAbility : GenericUpgrade
    {
        public EzraBridgerPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Ezra Bridger Pilot Ability",
                UpgradeType.Pilot,

                cost: 12,
                abilityType: typeof(Abilities.SecondEdition.EzraBridgerPilotAbility),
                addForce: 1
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/ezrabridger.png";
        }


    }
}