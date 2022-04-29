using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class JanOrsPilotAbility : GenericUpgrade
    {
        public JanOrsPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Jan Ors Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                abilityType: typeof(Abilities.SecondEdition.JanOrsAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/janors.png";
        }


    }
}