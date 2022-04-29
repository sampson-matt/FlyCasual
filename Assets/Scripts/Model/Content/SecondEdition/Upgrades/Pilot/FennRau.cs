using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class FennRauPilotAbility : GenericUpgrade
    {
        public FennRauPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Fenn Rau Pilot Ability",
                UpgradeType.Pilot,

                cost: 12,
                abilityType: typeof(Abilities.SecondEdition.FennRauRebelFangAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/fennrau.png";
        }


    }
}