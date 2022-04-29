using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class WullffwarroPilotAbility : GenericUpgrade
    {
        public WullffwarroPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Wullffwarro Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.WullffwarroAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/wullffwarro.png";
        }


    }
}