﻿using Ship;
using SubPhases;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESkStriker
    {
        public class Duchess : TIESkStriker
        {
            public Duchess() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Duchess\"",
                    5,
                    44,
                    isLimited: true,
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}