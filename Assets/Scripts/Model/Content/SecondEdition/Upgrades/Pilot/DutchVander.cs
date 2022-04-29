using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class DutchVanderPilotAbility : GenericUpgrade
    {
        public DutchVanderPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Dutch Vander Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.DutchVanderAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/dutchvander.png";
        }


    }
}