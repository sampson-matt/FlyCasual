﻿using Abilities.SecondEdition;
using BoardTools;
using Ship;
using System;
using System.Collections.Generic;
using Tokens;
using Upgrade;

namespace Ship.SecondEdition.TIEInterceptor
{
    public class CienaRee : TIEInterceptor
    {
        public CienaRee() : base()
        {
            PilotInfo = new PilotCardInfo(
                "Ciena Ree",
                6,
                47,
                isLimited: true,
                abilityType: typeof(CienaReeAbility),
                extraUpgradeIcon: UpgradeType.Talent
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CienaReeAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnAttackFinishAsAttacker += CheckKillAbility;
            GenericShip.OnShipIsDestroyedGlobal += CheckDestroyedAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackFinishAsAttacker -= CheckKillAbility;
            GenericShip.OnShipIsDestroyedGlobal -= CheckDestroyedAbility;
        }

        private void CheckKillAbility(GenericShip ship)
        {
            if (Combat.Defender.IsDestroyed) RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, GetStress);
        }

        private void GetStress(object sender, EventArgs e)
        {
            Messages.ShowInfo($"{HostShip.PilotInfo.PilotName} gains 1 stress token");
            HostShip.Tokens.AssignToken(typeof(StressToken), Triggers.FinishTrigger);
        }

        private void CheckDestroyedAbility(GenericShip ship, bool flag)
        {
            if (Tools.IsFriendly(HostShip, ship))
            {
                DistanceInfo distInfo = new DistanceInfo(HostShip, ship);
                if (distInfo.Range <= 3)
                {
                    RegisterAbilityTrigger(TriggerTypes.OnShipIsDestroyed, RemoveStressToken);
                }
            }
        }

        private void RemoveStressToken(object sender, EventArgs e)
        {
            if (HostShip.Tokens.HasToken<StressToken>())
            {
                Messages.ShowInfo($"{HostShip.PilotInfo.PilotName} removes 1 stress token");
                HostShip.Tokens.RemoveToken(typeof(StressToken), Triggers.FinishTrigger);
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }
    }
}