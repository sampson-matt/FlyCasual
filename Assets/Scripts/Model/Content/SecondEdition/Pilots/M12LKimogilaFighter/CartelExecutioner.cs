using Upgrade;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.M12LKimogilaFighter
    {
        public class CartelExecutioner : M12LKimogilaFighter
        {
            public CartelExecutioner() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Cartel Executioner",
                    3,
                    41,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Talent },
                    seImageNumber: 209
                );

                ModelInfo.SkinName = "Cartel Executioner";
            }
        }
    }
}