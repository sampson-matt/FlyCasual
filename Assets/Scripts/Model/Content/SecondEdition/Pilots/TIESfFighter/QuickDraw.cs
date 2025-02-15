﻿using Arcs;
using Ship;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESfFighter
    {
        public class Quickdraw : TIESfFighter
        {
            public Quickdraw() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Quickdraw\"",
                    6,
                    43,
                    isLimited: true,
                    extraUpgradeIcon: UpgradeType.Talent,
                    abilityType: typeof(Abilities.SecondEdition.QuickdrawAbility),
                    charges: 1,
                    regensCharges: 1
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class QuickdrawAbility : Abilities.FirstEdition.QuickDrawPilotAbility
    {
        protected override bool IsAbilityCanBeUsed()
        {
            if (HostShip.State.Charges == 0 || HostShip.IsCannotAttackSecondTime) return false;

            return true;
        }

        protected override void MarkAbilityAsUsed()
        {
            HostShip.SpendCharge();
        }
    }
}
