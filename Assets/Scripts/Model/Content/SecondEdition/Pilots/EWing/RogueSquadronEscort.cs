﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.EWing
    {
        public class RogueSquadronEscort : EWing
        {
            public RogueSquadronEscort() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Rogue Squadron Escort",
                    4,
                    51,
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}
