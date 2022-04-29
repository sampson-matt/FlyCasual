using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class BraylenStrammPilotAbility : GenericUpgrade
    {
        public BraylenStrammPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Braylen Stramm Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.BraylenStrammAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/braylenstramm.png";
        }


    }
}