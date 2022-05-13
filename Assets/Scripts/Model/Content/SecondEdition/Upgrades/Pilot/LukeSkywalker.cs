using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class LukeSkywalkerPilotAbility : GenericUpgrade
    {
        public LukeSkywalkerPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Luke Skywalker Pilot Ability",
                UpgradeType.Pilot,

                cost: 25,
                abilityType: typeof(Abilities.SecondEdition.LukeSkywalkerAbility),
                addForce: 2
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/lukeskywalker.png";
        }


    }
}