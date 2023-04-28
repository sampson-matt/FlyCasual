using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.NimbusClassVWing
    {
        public class ShadowSquadronEscort : NimbusClassVWing
        {
            public ShadowSquadronEscort() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Shadow Squadron Escort",
                    3,
                    28,
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}