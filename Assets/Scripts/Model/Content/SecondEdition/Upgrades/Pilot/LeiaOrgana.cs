using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class LeiaOrganaPilotAbility : GenericUpgrade
    {
        public LeiaOrganaPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Leia Organa Pilot Ability",
                UpgradeType.Pilot,

                cost: 20,
                abilityType: typeof(Abilities.SecondEdition.LeiaOrganaPilotAbility),
                addForce: 1
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/leiaorgana.png";
        }


    }
}