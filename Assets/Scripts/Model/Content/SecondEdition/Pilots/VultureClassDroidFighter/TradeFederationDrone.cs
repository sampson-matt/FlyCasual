using System;
using System.Collections.Generic;

namespace Ship.SecondEdition.VultureClassDroidFighter
{
    public class TradeFederationDrone : VultureClassDroidFighter
    {
        public TradeFederationDrone()
        {
            PilotInfo = new PilotCardInfo(
                "Trade Federation Drone",
                1,
                21
            );
        }
    }
}