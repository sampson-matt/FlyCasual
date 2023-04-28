using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    namespace SecondEdition.T70XWing
    {
        public class BlueSquadronRookie : T70XWing
        {
            public BlueSquadronRookie() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Blue Squadron Rookie",
                    1,
                    42
                );
            }
        }
    }
}
