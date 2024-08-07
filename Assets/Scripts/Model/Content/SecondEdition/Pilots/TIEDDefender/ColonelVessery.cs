﻿using Upgrade;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.TIEDDefender
    {
        public class ColonelVessery : TIEDDefender
        {
            public ColonelVessery() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Colonel Vessery",
                    4,
                    78,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.ColonelVesseryAbility),
                    extraUpgradeIcons: new List<UpgradeType>(){ UpgradeType.Talent, UpgradeType.Sensor }
                );
            }
        }
    }
}