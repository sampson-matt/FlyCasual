using System.Collections.Generic;
using Arcs;
using Upgrade;
using System;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class ProtonCannons : GenericSpecialWeapon
    {
        public ProtonCannons() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Proton Cannons",
                types: new List<UpgradeType>()
                {
                    UpgradeType.Cannon,
                    UpgradeType.Cannon
                },
                cost: 5,
                weaponInfo: new SpecialWeaponInfo(
                    attackValue: 4,
                    minRange: 2,
                    maxRange: 3,
                    charges: 2,
                    regensCharges: true,
                    arc: ArcType.Bullseye
                ),
                abilityType: typeof(Abilities.SecondEdition.ProtonCannonsAbility)

            );

            ImageUrl = "https://infinitearenas.com/xw2/images/upgrades/protoncannons.png";
        }
        public override void PayAttackCost(Action callBack) {
            State.SpendCharges(2);
            callBack();
        }
        public override bool IsShotAvailable(GenericShip targetShip)
        {
            return base.IsShotAvailable(targetShip) && State.Charges >= 2;
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ProtonCannonsAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostUpgrade.UpgradeInfo.Name,
                IsDiceModificationAvailable,
                GetDiceModificationAiPriority,
                DiceModificationType.Change,
                1,
                new List<DieSide>() { DieSide.Focus, DieSide.Success  },
                DieSide.Crit
            );
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }        

        private bool IsDiceModificationAvailable()
        {
            bool result = true;

            if (Combat.AttackStep != CombatStep.Attack) result = false;

            if (Combat.ChosenWeapon != HostUpgrade) result = false;

            return result;
        }

        private int GetDiceModificationAiPriority()
        {
            int result = 0;
            if (Combat.DiceRollAttack.RegularSuccesses + Combat.DiceRollAttack.Focuses > 0) result = 100;
            return result;
        }
    }
}
