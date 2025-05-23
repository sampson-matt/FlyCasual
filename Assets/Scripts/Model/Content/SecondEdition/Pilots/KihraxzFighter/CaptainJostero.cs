using BoardTools;
using Ship;
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
        private bool IsCannotAttackSecondTimePreviousValue;
        private GenericShip triggeringShip;
        private GenericShip activeShip;
        private GenericShip previousAttacker = null;

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
            triggeringShip = damaged;

            // Can we even bonus attack?
            if (HostShip.IsCannotAttackSecondTime)
                return;

            // Make sure the opposing ship is an enemy.
            if (Tools.IsSameTeam(damaged, HostShip))
                return;

            // If the ship is defending we're not interested.
            if (Combat.Defender == damaged || damage.DamageType == DamageTypes.ShipAttack)
                return;

            //ShotInfo arcInfo = new ShotInfo(HostShip, damaged, HostShip.PrimaryWeapons);
            //if (!arcInfo.InArc || arcInfo.Range > 3)
            //    return;

            // Save the value for whether we've attacked or not.
            performedRegularAttack = HostShip.IsAttackPerformed;
            activeShip = Selection.ActiveShip;
            IsCannotAttackSecondTimePreviousValue = HostShip.IsCannotAttackSecondTime;

            

            // It may be possible in the future for a non-defender to be damaged in combat so we've got to future proof here.
            if (Combat.AttackStep == CombatStep.None || Combat.Attacker != HostShip)
            {
                previousAttacker = Combat.Attacker;
                string Name = HostShip.PilotInfo.PilotName + "'s ability ShipId:" + triggeringShip.ShipId;
                RegisterAbilityTrigger(TriggerTypes.OnDamageInstanceResolved, RegisterBonusAttack, customTriggerName: Name);
            }
            else
            {
                Combat.Attacker.OnCombatCheckExtraAttack += StartBonusAttack;
            }
        }

        private void StartBonusAttack(GenericShip ship)
        {
            ship.OnCombatCheckExtraAttack -= StartBonusAttack;
            string Name = HostShip.PilotInfo.PilotName + "'s ability ShipId:" + triggeringShip.ShipId;
            RegisterAbilityTrigger(TriggerTypes.OnCombatCheckExtraAttack, RegisterBonusAttack, customTriggerName: Name);
        }

        private void RegisterBonusAttack(object sender, System.EventArgs e)
        {
            HostShip.IsCannotAttackSecondTime = true;

            Combat.StartSelectAttackTarget(
                HostShip,
                CleanupBonusAttack,
                IsTargetShip,
                HostShip.PilotInfo.PilotName,
                "You may perform a bonus attack against " + triggeringShip.PilotInfo.PilotName + "("+triggeringShip.ShipId+")",
                HostShip
            );

        }

        private bool IsTargetShip(GenericShip defender, IShipWeapon weapon, bool isSilent)
        {
            if (defender == triggeringShip)
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
            if (HostShip.IsAttackSkipped)
            {
                HostShip.IsCannotAttackSecondTime = IsCannotAttackSecondTimePreviousValue;
            }
            // Restore previous value of "has already attacked" flag
            HostShip.IsAttackPerformed = performedRegularAttack;

            // Restore ship selection
            Selection.ChangeActiveShip(activeShip);
            Combat.Attacker = previousAttacker;

            Triggers.FinishTrigger();
        }
    }
}

