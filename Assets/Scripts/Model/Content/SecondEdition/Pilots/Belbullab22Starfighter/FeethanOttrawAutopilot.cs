using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship.SecondEdition.Belbullab22Starfighter
{
    public class FeethanOttrawAutopilot : Belbullab22Starfighter
    {
        public FeethanOttrawAutopilot()
        {
            PilotInfo = new PilotCardInfo(
                "Feethan Ottraw Autopilot",
                1,
                35
            );

            ShipInfo.ActionIcons.SwitchToDroidActions();
        }
    }
}