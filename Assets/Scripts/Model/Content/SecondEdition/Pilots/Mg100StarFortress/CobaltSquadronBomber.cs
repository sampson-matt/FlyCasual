using Ship;
using System.Collections;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.Mg100StarFortress
    {
        public class CobaltSquadronBomber : Mg100StarFortress
        {
            public CobaltSquadronBomber() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Cobalt Squadron Bomber",
                    1,
                    50
                );

                ModelInfo.SkinName = "Cobalt";
            }
        }
    }
}