using Content;
using System.Collections.Generic;

namespace Ship.SecondEdition.Belbullab22Starfighter
{
    public class FeethanOttrawAutopilot : Belbullab22Starfighter
    {
        public FeethanOttrawAutopilot()
        {
            PilotInfo = new PilotCardInfo(
                "Feethan Ottraw Autopilot",
                1,
                34,
                tags: new List<Content.Tags>
                {
                    Tags.Droid
                }
            );

            ShipInfo.ActionIcons.SwitchToDroidActions();
        }
    }
}