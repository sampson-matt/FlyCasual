using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.HMPDroidGunship
    {
        public class BaktoidDrone : HMPDroidGunship
        {
            public BaktoidDrone() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Baktoid Drone",
                    1,
                    37,
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Crew, UpgradeType.Device }
                );
            }

            
        }
    }
}