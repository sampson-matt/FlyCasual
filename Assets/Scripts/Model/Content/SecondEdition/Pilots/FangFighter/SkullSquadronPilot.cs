using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.FangFighter
    {
        public class SkullSquadronPilot : FangFighter
        {
            public SkullSquadronPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Skull Squadron Pilot",
                    4,
                    46,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Modification },
                    seImageNumber: 159
                );

                ModelInfo.SkinName = "Skull Squadron Pilot";
            }
        }
    }
}