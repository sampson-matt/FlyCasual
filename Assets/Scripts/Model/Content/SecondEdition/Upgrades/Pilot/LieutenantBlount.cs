using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class LieutenantBlountPilotAbility : GenericUpgrade
    {
        public LieutenantBlountPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Lieutenant Blount Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.LtBlountAbiliity)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Pilot-Abilities/main/PilotAbilities/Rebel/lieutenantblount.png";
        }


    }
}