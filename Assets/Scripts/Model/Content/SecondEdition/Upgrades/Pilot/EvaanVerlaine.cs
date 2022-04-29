using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class EvaanVerlainePilotAbility : GenericUpgrade
    {
        public EvaanVerlainePilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Evaan Verlaine Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                abilityType: typeof(Abilities.SecondEdition.EvaanVerlaineAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/evaanverlaine.png";
        }


    }
}