using Ship;
using System.Collections;
using System.Collections.Generic;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.YT2400LightFreighter2023
    {
        public class Leebo2023 : YT2400LightFreighter2023
        {
            public Leebo2023() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Leebo",
                    3,
                    71,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.LeeboAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Crew);

                PilotNameCanonical = "leebo-swz103";
                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/leebo-wisdomofages.png";
            }
        }
    }
}
