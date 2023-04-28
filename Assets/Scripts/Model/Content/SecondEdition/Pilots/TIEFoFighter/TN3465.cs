using BoardTools;
using System;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEFoFighter
    {
        public class TN3465 : TIEFoFighter
        {
            public TN3465() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "TN-3465",
                    2,
                    28,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.TN3465Ability)
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class TN3465Ability : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostShip.PilotInfo.PilotName,
                IsDiceModificationAvailable,
                GetDiceModificationAiPriority,
                DiceModificationType.Change,
                1,
                sideCanBeChangedTo: DieSide.Crit,
                isGlobal: true,
                payAbilityCost: SufferCriticalDamage
            );
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        private bool IsDiceModificationAvailable()
        {
            if (Combat.AttackStep != CombatStep.Attack) return false;

            if (Combat.Attacker.ShipId == HostShip.ShipId) return false;

            if (!Tools.IsFriendly(Combat.Attacker, HostShip)) return false;

            DistanceInfo distInfo = new DistanceInfo(HostShip, Combat.Defender);
            if (distInfo.Range > 1) return false;

            return true;
        }

        private int GetDiceModificationAiPriority()
        {
            return (HostShip.State.ShieldsCurrent > 0) ? 100 : 0;
        }

        private void SufferCriticalDamage(Action<bool> callback)
        {
            DamageSourceEventArgs damageArgs = new DamageSourceEventArgs()
            {
                DamageType = DamageTypes.CardAbility,
                Source = HostShip
            };

            HostShip.Damage.TryResolveDamage(0, 1, damageArgs, delegate { callback(true); });
        }
    }
}
