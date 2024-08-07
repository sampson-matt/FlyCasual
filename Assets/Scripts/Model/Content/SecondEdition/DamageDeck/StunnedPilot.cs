﻿using Ship;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageDeckCardSE
{

    public class StunnedPilot : GenericDamageCard
    {
        public StunnedPilot()
        {
            Name = "Stunned Pilot";
            Type = CriticalCardType.Pilot;
            ImageUrl = "https://i.imgur.com/Se6krBQ.png";
        }

        public override void ApplyEffect(object sender, EventArgs e)
        {
            Host.OnMovementFinish += RegisterCheckCollisionDamage;
            Host.Tokens.AssignCondition(typeof(Tokens.StunnedPilotSECritToken));
            Triggers.FinishTrigger();
        }

        private void RegisterCheckCollisionDamage(GenericShip ship)
        {
            if (Host.IsLandedOnObstacle || Host.IsHitObstacles)
            {
                Triggers.RegisterTrigger(new Trigger()
                {
                    Name = "Stunned Pilot crit",
                    TriggerType = TriggerTypes.OnMovementFinish,
                    TriggerOwner = Host.Owner.PlayerNo,
                    EventHandler = CheckCollisionDamage
                });
            }
        }

        private void CheckCollisionDamage(object sender, System.EventArgs e)
        {
            Messages.ShowInfo("Stunned Pilot has caused " + Host.PilotInfo.PilotName + " to suffer 1 Hit");

            DamageSourceEventArgs stunnedpilotDamage = new DamageSourceEventArgs()
            {
                Source = this,
                DamageType = DamageTypes.CriticalHitCard
            };

            Host.Damage.TryResolveDamage(1, stunnedpilotDamage, Triggers.FinishTrigger);
        }

        public override void DiscardEffect()
        {
            base.DiscardEffect();

            Host.OnMovementFinish -= RegisterCheckCollisionDamage;
            Host.Tokens.RemoveCondition(typeof(Tokens.StunnedPilotSECritToken));
        }

    }

}

namespace Tokens
{
    public class StunnedPilotSECritToken : CritToken
    {
        public StunnedPilotSECritToken(GenericShip host) : base(host)
        {
            Tooltip = "https://i.imgur.com/Se6krBQ.png";
        }
    }
}

