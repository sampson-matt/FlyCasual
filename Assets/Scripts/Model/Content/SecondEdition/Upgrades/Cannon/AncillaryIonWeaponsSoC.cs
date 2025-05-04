using Arcs;
using BoardTools;
using Ship;
using System;
using System.Linq;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class AncillaryIonWeaponsSoC : GenericUpgrade
    {
        public AncillaryIonWeaponsSoC() : base()
        {
            IsHidden = true;

            UpgradeInfo = new UpgradeCardInfo(
                "Ancillary Ion Weapons",
                UpgradeType.Cannon,
                cost: 0,
                charges: 2,
                regensCharges: true,
                abilityType: typeof(Abilities.SecondEdition.AncillaryIonWeaponsSoCAbility)
            );          
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class AncillaryIonWeaponsSoCAbility : Alpha3EEskAbility
    {
        private GenericShip Defender;
        public override void ActivateAbility()
        {
            HostShip.OnAttackStartAsAttacker += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackStartAsAttacker += CheckAbility;
        }

        protected override void CheckAbility()
        {
            if(HostUpgrade.State.Charges >= 2
                && Combat.AttackStep == CombatStep.Attack
                && Combat.Attacker == HostShip
                && Combat.ChosenWeapon.WeaponType == WeaponTypes.PrimaryWeapon
                && Combat.Attacker.SectorsInfo.IsShipInSector(Combat.Defender, ArcType.Front))
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackStart, RegisterEskAbility);
            }
        }
    }
}