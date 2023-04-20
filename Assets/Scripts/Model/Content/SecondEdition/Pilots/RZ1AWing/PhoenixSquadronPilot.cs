using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ1AWing
    {
        public class PhoenixSquadronPilot : RZ1AWing
        {
            public PhoenixSquadronPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Phoenix Squadron Pilot",
                    1,
                    28,
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ModelInfo.SkinName = "Phoenix Squadron";
            }
        }
    }
}