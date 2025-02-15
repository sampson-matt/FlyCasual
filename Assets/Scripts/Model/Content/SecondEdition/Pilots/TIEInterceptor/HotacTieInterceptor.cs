﻿using Ship;
using System;
using System.Collections.Generic;
using Upgrade;
using Mods.ModsList;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class HotacInterceptorPilot : TIEInterceptor
        {
            public HotacInterceptorPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac Interceptor Pilot",
                    1,
                    0,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Pilot, UpgradeType.Torpedo, UpgradeType.Modification, UpgradeType.Init }
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/alphasquadronpilot.png";

                RequiredMods = new List<Type>() { typeof(HotacEliteImperialPilotsModSE) };
            }
        }
    }
}