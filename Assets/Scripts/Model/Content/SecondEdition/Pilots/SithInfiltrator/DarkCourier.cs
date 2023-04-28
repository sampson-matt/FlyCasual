using System;
using System.Collections.Generic;
using Ship;
using Upgrade;

namespace Ship.SecondEdition.SithInfiltrator
{
    public class DarkCourier : SithInfiltrator
    {
        public DarkCourier()
        {
            PilotInfo = new PilotCardInfo(
                "Dark Courier",
                2,
                47
            );
        }
    }
}