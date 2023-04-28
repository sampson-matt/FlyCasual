using BoardTools;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ship.SecondEdition.ResistanceTransport
{
    public class LogisticsDivisionPilot : ResistanceTransport
    {
        public LogisticsDivisionPilot()
        {
            PilotInfo = new PilotCardInfo(
                "Logistics Division Pilot",
                1,
                30
            );
        }
    }
}