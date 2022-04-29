using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class WedgeAntillesAwingPilotAbility : GenericUpgrade
    {
        public WedgeAntillesAwingPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Wedge Antilles Awing Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.WedgeAntillesAWingAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/wedgeantillesawing.png";
        }


    }
}