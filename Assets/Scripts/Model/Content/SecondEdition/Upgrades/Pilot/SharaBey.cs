using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class SharaBeyPilotAbility : GenericUpgrade
    {
        public SharaBeyPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Shara Bey Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.SharaBeyAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/sharabey.png";
        }


    }
}