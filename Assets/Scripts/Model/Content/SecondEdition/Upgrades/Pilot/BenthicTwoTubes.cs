using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class BenthicTwoTubesPilotAbility : GenericUpgrade
    {
        public BenthicTwoTubesPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Benthic Two Tubes Pilot Ability",
                UpgradeType.Pilot,

                cost: 4,
                abilityType: typeof(Abilities.FirstEdition.BenthicTwoTubesAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/benthictwotubes.png";
        }


    }
}