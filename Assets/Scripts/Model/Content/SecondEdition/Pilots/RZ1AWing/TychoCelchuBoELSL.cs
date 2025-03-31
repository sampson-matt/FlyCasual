using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Arcs;
using Content;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ1AWing
    {
        public class TychoCelchuBoELSL : RZ1AWing
        {
            public TychoCelchuBoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Tycho Celchu",
                    5,
                    41,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.TychoCelchuBattleOverEndorAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE
                    },
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Talent }
                );
                PilotNameCanonical = "tychocelchu-battleoverendor-lsl";
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(FocusAction), typeof(ReloadAction)));
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BoostAction), typeof(EvadeAction)));
                ShipInfo.ArcInfo.Arcs.Add(new ShipArcInfo(ArcType.SingleTurret, 2));
                ShipInfo.ArcInfo.Arcs.RemoveAll(n => n.ArcType == ArcType.Front);
                VectoredThrustersAbility oldAbility = (VectoredThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(VectoredThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new VectoredCannonsAbility());
            }            
        }
    }
}

namespace Abilities.SecondEdition
{
    public class TychoCelchuBattleOverEndorAbility : GenericAbility
    {
        //While you are disarmed, you can still perform missile attacks. When you perform a missile attack while disarmed, roll a maximum of 4 dice.
        GenericSpecialWeapon secondaryWeapon;
        public override void ActivateAbility()
        {
            HostShip.OnWeaponsDisabledCheck += AllowMissileAttacks;
        }
        public override void DeactivateAbility()
        {
            HostShip.OnWeaponsDisabledCheck -= AllowMissileAttacks;        
        }

        private void AllowMissileAttacks(ref bool result)
        {
            if (HostShip.Tokens.CountTokensByType(typeof(WeaponsDisabledToken)) != 1) return;

            if (!IsMissileAttack()) return;

            Messages.ShowInfo("The attack using " + secondaryWeapon.Name + " is allowed");
            result = false;
            PrepareAttackDiceCap();
        }

        private bool IsMissileAttack()
        {
            bool result = false;

            secondaryWeapon = Combat.ChosenWeapon as GenericSpecialWeapon;
            if (secondaryWeapon != null)
            {
                if (secondaryWeapon.HasType(UpgradeType.Missile))
                {
                    result = true;
                }
            }

            return result;
        }

        private void PrepareAttackDiceCap()
        {
            HostShip.AfterGotNumberOfAttackDiceCap += SetAttackDiceCap;

            HostShip.OnAttackFinish += RemoveAttackDiceCap;
        }

        private void SetAttackDiceCap(ref int count)
        {
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + " has a disarmed token, only 4 dice may be rolled when attacking with " + secondaryWeapon.Name);
            if (count > 4) count = 4;
        }

        private void RemoveAttackDiceCap(GenericShip ship)
        {
            HostShip.AfterGotNumberOfAttackDiceCap -= SetAttackDiceCap;

            HostShip.OnAttackFinish -= RemoveAttackDiceCap;
        }
    }
}