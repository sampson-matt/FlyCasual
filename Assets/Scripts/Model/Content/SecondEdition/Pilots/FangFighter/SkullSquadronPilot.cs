using System.Collections.Generic;
using Upgrade;
using Content;

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
                    tags: new List<Tags>
                    {
                        Tags.Mandalorian
                    },
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Modification }
                );

                ModelInfo.SkinName = "Skull Squadron Pilot";
            }
        }
    }
}