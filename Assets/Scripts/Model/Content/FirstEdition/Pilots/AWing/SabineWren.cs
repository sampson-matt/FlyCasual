﻿using System.Collections;
using System.Collections.Generic;
using Mods.ModsList;
using Upgrade;

namespace Ship
{
    namespace FirstEdition.AWing
    {
        public class SabineWren : AWing
        {
            public SabineWren() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sabine Wren",
                    5,
                    23,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.HeraSyndullaAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                IsHidden = true;

                ImageUrl = "https://i.imgur.com/yRrheRR.png";

                ModelInfo.SkinName = "Phoenix Squadron";
            }
        }
    }
}