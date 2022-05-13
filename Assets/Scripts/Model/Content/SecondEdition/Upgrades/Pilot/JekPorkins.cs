using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class JekPorkinsPilotAbility : GenericUpgrade
    {
        public JekPorkinsPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Jek Porkins Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.JekPorkinsAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/jekporkins.png";
        }


    }
}