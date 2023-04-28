using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.HMPDroidGunship
    {
        public class SeparatistPredator : HMPDroidGunship
        {
            public SeparatistPredator() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Separatist Predator",
                    3,
                    38,
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Crew, UpgradeType.Device }
                );
            }

            
        }
    }
}