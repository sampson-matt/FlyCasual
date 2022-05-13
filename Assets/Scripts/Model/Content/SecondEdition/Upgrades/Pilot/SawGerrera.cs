using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class SawGerreraPilotAbility : GenericUpgrade
    {
        public SawGerreraPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Saw Gerrera Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.SawGerreraPilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/sawgerrera.png";
        }


    }
}