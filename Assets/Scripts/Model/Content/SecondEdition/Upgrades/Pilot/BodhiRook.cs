using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class BodhiRookPilotAbility : GenericUpgrade
    {
        public BodhiRookPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Bodhi Rook Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.BodhiRookAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/bodhirook.png";
        }


    }
}