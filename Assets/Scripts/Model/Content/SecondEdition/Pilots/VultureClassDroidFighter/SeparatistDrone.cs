using System;
using System.Collections.Generic;

namespace Ship.SecondEdition.VultureClassDroidFighter
{
    public class SeparatistDrone : VultureClassDroidFighter
    {
        public SeparatistDrone()
        {
            PilotInfo = new PilotCardInfo(
                "Separatist Drone",
                3,
                22
            );
        }
    }
}