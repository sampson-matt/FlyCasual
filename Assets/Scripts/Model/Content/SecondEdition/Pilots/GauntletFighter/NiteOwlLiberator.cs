﻿
using Content;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class NiteOwlLiberator : GauntletFighter
        {
            public NiteOwlLiberator() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Nite Owl Liberator",
                    2,
                    52,
                    pilotTitle: "Resolute Warrior",
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.Mandalorian
                    },
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Illicit }
                );

                ModelInfo.SkinName = "Blue";
            }
        }
    }
}