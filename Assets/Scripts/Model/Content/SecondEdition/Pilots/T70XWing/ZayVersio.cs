using Movement;
using Ship;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.T70XWing
    {
        public class ZayVersio : T70XWing
        {
            public ZayVersio() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Zay Versio",
                    3,
                    46,
                    pilotTitle: "Her Father's Daughter",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.ZayVersioAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ZayVersioAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostName,
                IsAvailable,
                GetAiPriority,
                DiceModificationType.Reroll,
                1
                );
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        private int GetAiPriority()
        {
            return 90;
        }

        private bool IsAvailable()
        {
            return Combat.Defender == HostShip
                && Combat.AttackStep == CombatStep.Defence
                && Combat.Attacker.Damage.IsDamaged;
        }
    }
}