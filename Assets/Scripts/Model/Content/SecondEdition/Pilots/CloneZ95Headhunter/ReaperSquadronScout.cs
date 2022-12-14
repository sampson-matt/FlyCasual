using Abilities.Parameters;
using ActionsList;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.CloneZ95Headhunter
    {
        public class ReaperSquadronScout : CloneZ95Headhunter
        {
            public ReaperSquadronScout() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Reaper Squadron Scout",
                    3,
                    24,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );

                ImageUrl = "https://infinitearenas.com/xw2legacy/images/pilots/reapersquadronscout.png";
            }
        }
    }
}