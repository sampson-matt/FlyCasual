using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class BiggsDarklighterPilotAbility : GenericUpgrade
    {
        public BiggsDarklighterPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Biggs Darklighter Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                abilityType: typeof(Abilities.SecondEdition.BiggsDarklighterAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/biggsdarklighter.png";
        }


    }
}