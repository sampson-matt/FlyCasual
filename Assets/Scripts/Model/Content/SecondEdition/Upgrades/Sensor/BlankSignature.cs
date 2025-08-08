using Tokens;
using System.Linq;
using Upgrade;
using System.Collections.Generic;
using System;

namespace UpgradesList.SecondEdition
{
    public class BlankSignature : GenericUpgrade
    {
        public BlankSignature() : base()
        {
            UpgradeInfo = new UpgradeCardInfo
            (
                "Blank Signature",
                UpgradeType.Sensor,
                cost: 0,
                charges: 1,
                regensCharges: true,
                abilityType: typeof(Abilities.SecondEdition.BlankSignatureAbility)
            );

            IsHidden = true;

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/BattleOverEndor/BlankSignature.jpg";
        }
    }
}

namespace Abilities.SecondEdition
{
    //While defending, if you are not locked by the attacker, you may spend 1 charge to change 1 focus result to an evade result.
    public class BlankSignatureAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostUpgrade.UpgradeInfo.Name,
                IsDiceModificationAvailable,
                GetDiceModificationAiPriority,
                DiceModificationType.Change,
                1,
                new List<DieSide> { DieSide.Focus },
                DieSide.Success,
                payAbilityCost: PayAbilityCost
            );
        }

        private void PayAbilityCost(Action<bool> callback)
        {
            
            if (HostUpgrade.State.Charges > 0)
            {
                HostUpgrade.State.SpendCharge();
                callback(true);
            }
            else
            {
                callback(false);
            }
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        public bool IsDiceModificationAvailable()
        {
            return (HostUpgrade.State.Charges > 0
                && Combat.AttackStep == CombatStep.Defence
                && Combat.CurrentDiceRoll.Focuses != 0
                && Combat.Defender == HostShip
                && !ActionsHolder.HasTargetLockOn(Combat.Attacker, HostShip));
        }

        public int GetDiceModificationAiPriority()
        {
            int result = 0;

            int attackSuccessesCancelable = Combat.DiceRollAttack.SuccessesCancelable;
            int defenceSuccesses = Combat.CurrentDiceRoll.Successes;
            if (attackSuccessesCancelable > defenceSuccesses)
            {
                int defenceFocuses = Combat.DiceRollDefence.Focuses;
                int numFocusTokens = Selection.ActiveShip.Tokens.CountTokensByType(typeof(FocusToken));
                if (numFocusTokens > 0 && defenceFocuses > 1)
                {
                    // Multiple focus results on our defense roll and we have a Focus token.  Use it instead of the ability.
                    result = 0;
                }
                else if (defenceFocuses > 0)
                {
                    // We don't have a focus token.  Better use the ability.
                    result = 45;
                }
            }

            return result;
        }
    } 
}