using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class EdrioTwoTubesPilotAbility : GenericUpgrade
    {
        public EdrioTwoTubesPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Edrio Two Tubes Pilot Ability",
                UpgradeType.Pilot,

                cost: 4,
                abilityType: typeof(Abilities.FirstEdition.EdrioTwoTubesAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/edriotwotubes.png";
        }


    }
}