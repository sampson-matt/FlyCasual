using System;
using System.Collections.Generic;
using Ship;
using SubPhases;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.NimbusClassVWing
    {
        public class OddBall : NimbusClassVWing
        {
            public OddBall() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Odd Ball\"",
                    5,
                    31,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.OddBallAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                PilotNameCanonical = "oddball-nimbusclassvwing";
            }
        }
    }
}