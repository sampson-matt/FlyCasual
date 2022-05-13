using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class HeraSyndullaPilotAbility : GenericUpgrade
    {
        public HeraSyndullaPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Hera Syndulla Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                abilityType: typeof(Abilities.FirstEdition.HeraSyndullaAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/herasyndulla.png";
        }


    }
}