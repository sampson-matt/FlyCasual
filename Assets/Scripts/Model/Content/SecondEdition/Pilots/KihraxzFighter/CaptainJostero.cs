﻿using Ship;
using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.KihraxzFighter
    {
        public class CaptainJostero : KihraxzFighter
        {
            public CaptainJostero() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Captain Jostero",
                    3,
                    41,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.CaptainJosteroAbility)
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CaptainJosteroAbility : GenericAbility
    {
        private bool performedRegularAttack;

        public override void ActivateAbility()
        {
            GenericShip.OnDamageInstanceResolvedGlobal += CheckJosteroAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnDamageInstanceResolvedGlobal -= CheckJosteroAbility;
        }

        private void CheckJosteroAbility(GenericShip damaged, DamageSourceEventArgs damage)
        {
            // Can we even bonus attack?
            if (HostShip.IsCannotAttackSecondTime)
                return;

            // Make sure the opposing ship is an enemy.
            if (Tools.IsSameTeam(damaged, HostShip))
                return;

            // If the ship is defending we're not interested.
            if (Combat.Defender == damaged || damage.DamageType == DamageTypes.ShipAttack)
                return;

            // Save the value for whether we've attacked or not.
            performedRegularAttack = HostShip.IsAttackPerformed;

            TargetShip = damaged;

            // It may be possible in the future for a non-defender to be damaged in combat so we've got to future proof here.
            if (Combat.AttackStep == CombatStep.None)
            {
                RegisterAbilityTrigger(TriggerTypes.OnDamageInstanceResolved, RegisterBonusAttack);
            }
            else
            {
                Combat.Attacker.OnCombatCheckExtraAttack += StartBonusAttack;
            }
        }

        private void StartBonusAttack(GenericShip ship)
        {
            ship.OnCombatCheckExtraAttack -= StartBonusAttack;
            RegisterAbilityTrigger(TriggerTypes.OnCombatCheckExtraAttack, RegisterBonusAttack);
        }

        private void RegisterBonusAttack(object sender, System.EventArgs e)
        {
            HostShip.StartBonusAttack(CleanupBonusAttack, IsTargetShip);
        }

        private bool IsTargetShip(GenericShip defender, IShipWeapon weapon, bool isSilent)
        {
            if (defender == TargetShip)
            {
                return true;
            }
            else
            {
                if (!isSilent) Messages.ShowErrorToHuman("Your bonus attack must be against the ship that just suffered damage");
                return false;
            }
        }

        private void CleanupBonusAttack()
        {
            // Restore previous value of "has already attacked" flag
            HostShip.IsAttackPerformed = performedRegularAttack;

            // Restore ship selection
            Selection.ChangeActiveShip(TargetShip);

            Triggers.FinishTrigger();
        }
    }
}

