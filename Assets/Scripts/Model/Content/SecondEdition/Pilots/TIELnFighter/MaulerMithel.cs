﻿using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class MaulerMithel : TIELnFighter
        {
            public MaulerMithel() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Mauler\" Mithel",
                    5,
                    29,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.MaulerMithelAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}