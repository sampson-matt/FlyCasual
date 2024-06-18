using Ship;
using System;
using System.Linq;
using Tokens;
using UnityEngine;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class HomingTorpedoes : GenericSpecialWeapon
    {
        public HomingTorpedoes() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Homing Torpedoes",
                UpgradeType.Torpedo,
                cost: 5,
                weaponInfo: new SpecialWeaponInfo(
                    attackValue: 4,
                    minRange: 1,
                    maxRange: 2,
                    requiresToken: typeof(BlueTargetLockToken),
                    charges: 2
                ),
                abilityType: typeof(Abilities.SecondEdition.HomingTorpedoesAbility)
            );
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class HomingTorpedoesAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnShotStartAsAttacker += RegisterOwnTrigger;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnShotStartAsAttacker -= RegisterOwnTrigger;
        }

        private void RegisterOwnTrigger()
        {
            if (Combat.ChosenWeapon == HostUpgrade)
            {
                RegisterAbilityTrigger(TriggerTypes.OnShotStart, AskDefender);
            }
        }

        private void AskDefender(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                UseIfHaveEnoughHP,
                SufferOneDamage,
                descriptionLong: "Do you want to suffer 1 critical damage damage? (If you do, skip the Attack and Defense Dice steps and the attack is treated as hitting)",
                imageHolder: HostUpgrade,
                requiredPlayer: Combat.Defender.Owner.PlayerNo
            );
        }

        private bool UseIfHaveEnoughHP()
        {
            return Combat.Defender.State.HullCurrent + Combat.Defender.State.ShieldsCurrent > 1;
        }

        private void SufferOneDamage(object sender, EventArgs e)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();

            Messages.ShowInfo(HostUpgrade.UpgradeInfo.Name + ": Defender chose to suffer 1 critical damage");

            Combat.Defender.Damage.TryResolveDamage(
                0,
                1,
                new DamageSourceEventArgs()
                {
                    DamageType = DamageTypes.CardAbility,
                    Source = HostUpgrade
                },
                SkipDice
            );
        }

        private void SkipDice()
        {
            Combat.SkipAttackDiceRollsAndHit = true;
            Triggers.FinishTrigger();
        }
    }
}
