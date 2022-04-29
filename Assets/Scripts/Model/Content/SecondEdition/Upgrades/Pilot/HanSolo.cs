using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class HanSoloPilotAbility : GenericUpgrade
    {
        public HanSoloPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Han Solo Pilot Ability",
                UpgradeType.Pilot,

                cost: 12,
                abilityType: typeof(Abilities.SecondEdition.HanSoloRebelPilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/hansolo.png";
        }


    }
}