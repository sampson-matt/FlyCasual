using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Content;
using Ship;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class YendorBoELSL : T65XWing
        {
            public YendorBoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Yendor",
                    5,
                    50,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.LsL
                    },
                    abilityType: typeof(Abilities.SecondEdition.YendorAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ShipAbilities.Add(new LockedSFoils());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(FocusAction), typeof(BoostAction)));
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(FocusAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(BoostAction)));
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);                
                PilotNameCanonical = "yendor-battleoverendor-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class YendorAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                "Yendor",
                IsAvailable,
                AiPriority,
                DiceModificationType.Reroll,
                1,
                sidesCanBeSelected: new List<DieSide>() { DieSide.Blank },
                payAbilityCost: PayAbilityCost
            );
        }

        private bool IsAvailable()
        {
            return Combat.AttackStep == CombatStep.Attack
                && Combat.ChosenWeapon.WeaponType == WeaponTypes.PrimaryWeapon
                && Combat.DiceRollAttack.Blanks > 0;
        }

        private int AiPriority()
        {
            return 75;
        }

        private void PayAbilityCost(Action<bool> callback)
        {
            HostShip.Tokens.AssignToken(typeof(Tokens.StrainToken), () => callback(true));
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }
    }
}
