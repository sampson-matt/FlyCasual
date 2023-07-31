using Ship;
using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.VT49Decimator
    {
        public class CaptainOicunn : VT49Decimator
        {
            public CaptainOicunn() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Captain Oicunn",
                    3,
                    70,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.CaptainOicunnAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ImageUrl = "https://github.com/eirikmun/x-wing2.0-project-goldenrod/blob/2.0/src/images/En/pilots/captainoicunn.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CaptainOicunnAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            HostShip.PrimaryWeapons.ForEach(n => n.WeaponInfo.MinRange = 0);
        }

        public override void DeactivateAbility()
        {
            HostShip.PrimaryWeapons.ForEach(n => n.WeaponInfo.MinRange = 1);
        }

    }
}
