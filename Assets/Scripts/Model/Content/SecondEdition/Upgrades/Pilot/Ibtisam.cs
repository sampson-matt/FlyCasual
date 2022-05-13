using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class IbtisamPilotAbility : GenericUpgrade
    {
        public IbtisamPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Ibtisam Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                abilityType: typeof(Abilities.FirstEdition.BraylenStrammAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/ibtisam.png";
        }


    }
}