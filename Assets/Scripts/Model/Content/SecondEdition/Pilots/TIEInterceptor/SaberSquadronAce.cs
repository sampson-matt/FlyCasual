﻿using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class SaberSquadronAce : TIEInterceptor
        {
            public SaberSquadronAce() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Saber Squadron Ace",
                    4,
                    37,
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ModelInfo.SkinName = "Red Stripes";
            }
        }
    }
}