using Arcs;
using Ship;
using System.Collections.Generic;
using System;
using Tokens;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class SeekerMissiles : GenericSpecialWeapon
    {
        public SeekerMissiles() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Seeker Missiles",
                types: new List<UpgradeType>(){
                    UpgradeType.Missile
                },
                cost: 5,
                limited: 2,
                weaponInfo: new SpecialWeaponInfo(
                    attackValue: 3,
                    minRange: 2,
                    maxRange: 3,
                    charges: 2,
                    requiresToken: typeof(BlueTargetLockToken)
                ),
                abilityType: typeof(Abilities.SecondEdition.SeekerMissilesAbility)
            );
            NameCanonical = "seekermissiles-rsl";
        }        
    }
}

namespace Abilities.SecondEdition
{
    //  If this attack misses and 1 or more hit/crit results were neutralized, the defender gains 1 strain token.
    public class SeekerMissilesAbility : GenericAbility
    {
        int hitOrCritResults;

        public override void ActivateAbility()
        {
            HostShip.OnDefenceStartAsAttacker += SaveHitOrCritResults;
            HostShip.OnAttackMissedAsAttacker += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnDefenceStartAsAttacker -= SaveHitOrCritResults;
            HostShip.OnAttackMissedAsAttacker -= RegisterAbility;
        }

        private void SaveHitOrCritResults()
        {
            hitOrCritResults = Combat.DiceRollAttack.Successes;
        }

        private void RegisterAbility()
        {
            if (hitOrCritResults > 0 && Combat.ChosenWeapon == HostUpgrade)
            {
                HostShip.OnAttackFinish += RegisterTrigger;
            }
        }

        private void RegisterTrigger(GenericShip ship)
        {
            HostShip.OnAttackFinish -= RegisterTrigger;
            RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, AssignStrainToDefender);
        }

        private void AssignStrainToDefender(object sender, System.EventArgs e)
        {
            Messages.ShowInfo(HostUpgrade.UpgradeInfo.Name + " assigned a strain token to " + Combat.Defender.PilotInfo.PilotName);
            Combat.Defender.Tokens.AssignToken(typeof(Tokens.StrainToken), Triggers.FinishTrigger);
        }
    }
}