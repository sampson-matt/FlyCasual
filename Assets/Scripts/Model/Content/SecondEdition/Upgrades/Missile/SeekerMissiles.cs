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
                    UpgradeType.Missile,
                    UpgradeType.Missile
                },
                cost: 7,
                weaponInfo: new SpecialWeaponInfo(
                    attackValue: 3,
                    minRange: 2,
                    maxRange: 3,
                    charges: 4,
                    requiresToken: typeof(BlueTargetLockToken)
                ),
                abilityType: typeof(Abilities.SecondEdition.SeekerMissilesAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/RSLUpgrades/seekermissiles.jpg";
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class SeekerMissilesAbility : GenericAbility
    {
        private int usedCount = 0;
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostUpgrade.UpgradeInfo.Name,
                IsAvailable,
                GetAiPriority,
                DiceModificationType.Change,
                1,
                sidesCanBeSelected: new List<DieSide>() { DieSide.Focus },
                sideCanBeChangedTo: DieSide.Success,
                canBeUsedFewTimes: true,
                payAbilityPostCost: PayAbilityCost
            );
        }

        private bool IsAvailable()
        {
            return Combat.ChosenWeapon == HostUpgrade
                && HostUpgrade.State.Charges > 0
                && usedCount < 2
                && Combat.AttackStep == CombatStep.Attack;
        }

        private int GetAiPriority()
        {
            return 39; // Just a bit lower than focus and calculate
        }

        private void PayAbilityCost()
        {
            usedCount++;
            HostUpgrade.State.SpendCharge();
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }
    }
}