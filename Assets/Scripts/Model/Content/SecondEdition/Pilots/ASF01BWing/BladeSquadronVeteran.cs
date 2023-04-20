using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ASF01BWing
    {
        public class BladeSquadronVeteran : ASF01BWing
        {
            public BladeSquadronVeteran() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Blade Squadron Veteran",
                    3,
                    41,
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/bladesquadronveteran.png";

                ModelInfo.SkinName = "Blue";
            }
        }
    }
}
