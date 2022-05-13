using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class AlexsandrKallusPilotAbility : GenericUpgrade
    {
        public AlexsandrKallusPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Alexsandr Kallus Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.AlexandrKallusAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Rebel/alexsandrkallus.png";
        }


    }
}