using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class WedgeAntillesPilotAbility : GenericUpgrade
    {
        public WedgeAntillesPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Wedge Antilles Pilot Ability",
                UpgradeType.Pilot,

                cost: 12,
                abilityType: typeof(Abilities.FirstEdition.WedgeAntillesAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/wedgeantilles.png";
        }


    }
}