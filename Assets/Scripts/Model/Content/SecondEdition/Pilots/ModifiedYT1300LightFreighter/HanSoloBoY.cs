using BoardTools;
using System;
using System.Collections;
using ActionsList;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ModifiedYT1300LightFreighter
    {
        public class HanSoloBoY : ModifiedYT1300LightFreighter
        {
            public HanSoloBoY() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Han Solo",
                    6,
                    84,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.HanSoloBoYPilotAbility),
                    charges: 4,
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/hansolo-boy.png";
                PilotNameCanonical = "hansolo-boy";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class HanSoloBoYPilotAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostShip.PilotInfo.PilotName,
                IsDiceModificationAvailable,
                GetAiPriority,
                DiceModificationType.Reroll,
                int.MaxValue,
                timing: DiceModificationTimingType.AfterRolled,
                isForcedFullReroll: true,
                payAbilityCost: PayAbilityCost
            );
            HostShip.OnAttackHitAsAttacker += RegisterHitAbility;
        }

        private void RegisterHitAbility()
        {
            if (HostShip.State.Charges > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackHit, AskUseAbility);
            }
        }

        private void AskUseAbility(object sender, EventArgs e)
        {
            HostShip.BeforeActionIsPerformed += RegisterSpendChargeTrigger;
            HostShip.AskPerformFreeAction(
                new CoordinateAction(),
                CleanUp,
                HostShip.PilotInfo.PilotName,
                "After you perform an attack that hits you may spend 1 Charge to perform a Coordinate action.",
                HostShip
            );
        }

        private void RegisterSpendChargeTrigger(GenericAction action, ref bool isFreeAction)
        {
            HostShip.BeforeActionIsPerformed -= RegisterSpendChargeTrigger;
            RegisterAbilityTrigger(
                TriggerTypes.OnFreeAction,
                delegate {
                    HostShip.SpendCharge();
                    Triggers.FinishTrigger();
                }
            );
        }

        private void CleanUp()
        {
            HostShip.BeforeActionIsPerformed -= RegisterSpendChargeTrigger;
            Triggers.FinishTrigger();
        }

        private bool IsDiceModificationAvailable()
        {
            return Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(0, 1), Team.Type.Friendly).Count == 1;
        }

        private int GetAiPriority()
        {
            if (HostShip.IsAttacking && Combat.DiceRollAttack.Failures > Combat.DiceRollAttack.Successes)
            {
                return 95;
            }
            else if (HostShip.IsDefending && Combat.DiceRollDefence.Failures > Combat.DiceRollDefence.Successes)
            {
                return 95;
            }
            else return 0;
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
            HostShip.OnAttackHitAsAttacker -= RegisterHitAbility;
        }

        private void PayAbilityCost(Action<bool> callback)
        {
            HostShip.SpendCharge();
            callback(true);
        }
    }
}

