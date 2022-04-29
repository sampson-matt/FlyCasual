using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class GarvenDreisPilotAbility : GenericUpgrade
    {
        public GarvenDreisPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Garven Dreis Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.GarvenDreisAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/garvendreis.png";
        }


    }
}