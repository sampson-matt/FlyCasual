using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTLBYWing
    {
        public class ShadowSquadronVeteran : BTLBYWing
        {
            public ShadowSquadronVeteran() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Shadow Squadron Veteran",
                    3,
                    31,
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Astromech }
                );
            }
        }
    }
}
