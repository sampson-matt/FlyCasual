using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ScavengedYT1300
    {
        public class ResistanceSympathizer : ScavengedYT1300
        {
            public ResistanceSympathizer() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Resistance Sympathizer",
                    2,
                    55
                );
            }
        }
    }
}