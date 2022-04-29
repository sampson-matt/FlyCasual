using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class KananJarrusPilotAbility : GenericUpgrade
    {
        public KananJarrusPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Kanan Jarrus Pilot Ability",
                UpgradeType.Pilot,

                cost: 15,
                abilityType: typeof(Abilities.SecondEdition.KananJarrusPilotAbility),
                addForce: 2
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/kananjarrus.png";
        }


    }
}