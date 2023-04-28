using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.T70XWing
    {
        public class RedSquadronExpert : T70XWing
        {
            public RedSquadronExpert() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Red Squadron Expert",
                    3,
                    45,
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ModelInfo.SkinName = "Red";
            }
        }
    }
}
