using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class CassianAndorPilotAbility : GenericUpgrade
    {
        public CassianAndorPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Cassian Andor Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                abilityType: typeof(Abilities.SecondEdition.CassianAndorAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/cassianandor.png";
        }


    }
}