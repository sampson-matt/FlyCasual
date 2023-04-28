

using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESeBomber
    {
        public class SienarJeamusTestPilot : TIESeBomber
        {
            public SienarJeamusTestPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sienar-Jaemus Test Pilot",
                    2,
                    31
                );
            }
        }
    }
}
