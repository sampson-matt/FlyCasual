using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class VultSkerris : TIEInterceptor
        {
            public VultSkerris() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Vult Skerris",
                    5,
                    44,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.VultSkerrisDefenderAbility),
                    extraUpgradeIcon: UpgradeType.Talent,
                    abilityText: " Action: Recover 1 charge take 1 strain. Before you engage you spend 1 charge to perform an action.",
                    charges: 1,
                    regensCharges: -1
                );

                PilotNameCanonical = "vultskerris-tieininterceptor";

                ModelInfo.SkinName = "Skystrike Academy";
            }
        }
    }
}