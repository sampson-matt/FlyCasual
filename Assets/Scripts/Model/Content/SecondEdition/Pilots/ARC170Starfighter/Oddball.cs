﻿using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ARC170Starfighter
    {
        public class OddBall : ARC170Starfighter
        {
            public OddBall() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Odd Ball\"",
                    5,
                    48,
                    isLimited: true,
                    factionOverride: Faction.Republic,
                    abilityType: typeof(Abilities.SecondEdition.OddBallAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                PilotNameCanonical = "oddball-arc170starfighter";

                ModelInfo.SkinName = "Red";
            }
        }
    }
}
