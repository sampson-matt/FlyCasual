using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ2AWing
    {
        public class GreenSquadronExpert : RZ2AWing
        {
            public GreenSquadronExpert() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Green Squadron Expert",
                    3,
                    33,
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Talent }
                );

                ModelInfo.SkinName = "Green";
            }
        }
    }
}