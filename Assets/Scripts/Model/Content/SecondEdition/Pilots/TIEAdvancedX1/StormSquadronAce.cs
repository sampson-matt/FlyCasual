﻿using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEAdvancedX1
    {
        public class StormSquadronAce : TIEAdvancedX1
        {
            public StormSquadronAce() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Storm Squadron Ace",
                    3,
                    37,
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}