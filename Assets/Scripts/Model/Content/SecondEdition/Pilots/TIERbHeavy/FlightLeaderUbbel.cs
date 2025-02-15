﻿using Upgrade;
using Ship;
using BoardTools;

namespace Ship
{
    namespace SecondEdition.TIERbHeavy
    {
        public class FlightLeaderUbbel : TIERbHeavy
        {
            public FlightLeaderUbbel() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Flight Leader Ubbel",
                    5,
                    42,
                    pilotTitle: "Onyx Leader",
                    abilityType: typeof(Abilities.SecondEdition.FlightLeaderUbbelAbility),
                    isLimited: true,
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class FlightLeaderUbbelAbility : GenericAbility
    {
        private GenericShip Attacker;
        private bool PerformedRegularAttack;
        private bool IsAlreadyRegistered;

        public override void ActivateAbility()
        {
            GenericShip.OnDamageCardIsDealtGlobal += TryRegisterAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnDamageCardIsDealtGlobal -= TryRegisterAbility;
        }

        protected void TryRegisterAbility(GenericShip ship)
        {
            if (Tools.IsFriendly(ship, HostShip)
                && Board.IsShipBetweenRange(HostShip, ship, 0, 2)
                && (Combat.Defender != null)
                && Tools.IsSameShip(ship, Combat.Defender)
                && !IsAlreadyRegistered
            )
            {
                Attacker = Combat.Attacker;
                Attacker.OnCombatCheckExtraAttack += StartBonusAttack;
                PerformedRegularAttack = HostShip.IsAttackPerformed;
                IsAlreadyRegistered = true;
            }
        }

        private void StartBonusAttack(GenericShip ship)
        {
            IsAlreadyRegistered = false;
            Attacker.OnCombatCheckExtraAttack -= StartBonusAttack;
            RegisterAbilityTrigger(TriggerTypes.OnCombatCheckExtraAttack, RegisterBonusAttack);
        }

        private void RegisterBonusAttack(object sender, System.EventArgs e)
        {
            if (!HostShip.IsCannotAttackSecondTime)
            {
                HostShip.IsCannotAttackSecondTime = true;

                Combat.StartSelectAttackTarget(
                    HostShip,
                    FinishBonusAttack,
                    CounterAttackFilter,
                    HostShip.PilotInfo.PilotName,
                    "You may perform a bonus attack",
                    HostShip
                );
            }
            else
            {
                Messages.ShowErrorToHuman(string.Format("{0} cannot perform second bonus attack", HostShip.PilotInfo.PilotName));
                FinishBonusAttack();
            }
        }

        private bool CounterAttackFilter(GenericShip targetShip, IShipWeapon weapon, bool isSilent)
        {
            bool result = true;

            if (targetShip != Attacker)
            {
                if (!isSilent) Messages.ShowErrorToHuman(string.Format("{0} can only attack {1}", HostShip.PilotInfo.PilotName, Attacker.PilotInfo.PilotName));
                result = false;
            }

            return result;
        }

        private void FinishBonusAttack()
        {
            // Restore previous value of "is already attacked" flag
            HostShip.IsAttackPerformed = PerformedRegularAttack;
            //if bonus attack was skipped, allow bonus attacks again
            if (HostShip.IsAttackSkipped) HostShip.IsCannotAttackSecondTime = false;
            Triggers.FinishTrigger();
        }
    }
}