using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class MirandaDoniPilotAbility : GenericUpgrade
    {
        public MirandaDoniPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Miranda Doni Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.MirandaDoniAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/mirandadoni.png";
        }


    }
}