using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class AhsokaTanoDelta7PilotAbility : GenericUpgrade
    {
        public AhsokaTanoDelta7PilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Ahsoka Tano Delta7 Pilot Ability",
                UpgradeType.Pilot,

                cost: 15,
                abilityType: typeof(Abilities.SecondEdition.AhsokaTanoAbility),
                addForce: 2
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Republic/ahsokatanodelta7.png";
        }


    }
}