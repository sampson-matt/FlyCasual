using ActionsList;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using UnityEngine;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class Kuiil : GenericUpgrade
    {
        public Kuiil() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Kuiil",
                UpgradeType.Crew,
                cost: 6,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Scum),
                abilityType: typeof(Abilities.SecondEdition.KuiilAbility)
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    //Roll 1 attack die for each damage card you have. 
    //For each Hit result, repair 1 faceup Ship damage card, 
    //then for each Critical Hit result, repair 1 facedown damage card.
    //For each blank result, remove 1 orange token,
    //then for each icon: action focus result, gain 1 focus token. 
    public class KuiilAbility : GenericAbility
    {
        public int hitIndex = 0;
        public int blankIndex = 0;

        public override void ActivateAbility()
        {
            HostShip.OnGenerateActions += AddAction;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnGenerateActions -= AddAction;
        }

        protected void AddAction(Ship.GenericShip ship)
        {
            ship.AddAvailableAction(new ActionsList.GenericAction()
            {
                ImageUrl = HostUpgrade.ImageUrl,
                HostShip = HostShip,
                Source = HostUpgrade,
                DoAction = DoAction,
                Name = HostName
            });
        }

        protected void DoAction()
        {
            PerformDiceCheck(
                HostName,
                DiceKind.Attack,
                HostShip.Damage.DamageCards.Count,
                CheckSuccesses,
                Phases.CurrentSubPhase.CallBack
            );
        }

        private void CheckSuccesses()
        {
            if (DiceCheckRoll.RegularSuccesses > 0 && HostShip.Damage.GetFaceupCrits(CriticalCardType.Ship).Any())
            {
                if (DiceCheckRoll.RegularSuccesses >= HostShip.Damage.GetFaceupCrits(CriticalCardType.Ship).Count)
                {
                    foreach (GenericDamageCard card in HostShip.Damage.GetFaceupCrits(CriticalCardType.Ship))
                    {
                        HostShip.Damage.FlipFaceupCritFacedown(card, delegate { });
                    }
                    CheckCrits(AbilityDiceCheck.ConfirmCheck);
                }
                else
                {
                    hitIndex = DiceCheckRoll.RegularSuccesses;
                    ChooseRepairFaceupCrits(AbilityDiceCheck.ConfirmCheck);
                }
            }
            else
            {
                CheckCrits(AbilityDiceCheck.ConfirmCheck);
            }
        }
        private void CheckCrits(Action callBack)
        {
            if (DiceCheckRoll.CriticalSuccesses > 0 && HostShip.Damage.HasFacedownCards)
            {
                for (int i = 0; i < DiceCheckRoll.CriticalSuccesses; i++)
                {
                    HostShip.Damage.DiscardRandomFacedownCard();
                    Messages.ShowInfo("Facedown damage card repaired.");
                }
            }
            CheckBlanks(callBack);
        }
        private void CheckBlanks(Action callBack)
        {
            if (DiceCheckRoll.Blanks > 0 && HostShip.Tokens.CountTokensByColor(TokenColors.Orange) > 0)
            {
                if (HostShip.Tokens.CountTokensByColor(TokenColors.Orange) <= DiceCheckRoll.Blanks)
                {
                    HostShip.Tokens.RemoveAllTokensByColor(TokenColors.Orange, () => CheckFocuses(callBack));
                    Messages.ShowInfo("Orange Tokens Removed");
                }
                else
                {
                    blankIndex = DiceCheckRoll.Blanks;
                    ChooseRemoveOrangeTokens(callBack);
                }
            }
            else
            {
                CheckFocuses(callBack);
            }            
        }
        private void CheckFocuses(Action callBack)
        {
            if (DiceCheckRoll.Focuses > 0)
            {
                HostShip.Tokens.AssignTokens(CreateFocusToken, DiceCheckRoll.Focuses, callBack);
            } else
            {
                callBack();
            }            
        }
        private GenericToken CreateFocusToken()
        {
            return new Tokens.FocusToken(HostShip);
        }        
        private void ChooseRepairFaceupCrits(Action callBack)
        {
            Action nextPhase = null;
            if(hitIndex == 1)
            {
                nextPhase = () => CheckCrits(callBack);
            } else
            {
                hitIndex--;
                nextPhase = () => ChooseRepairFaceupCrits(callBack);
            }

            KuiilCritDecisonSubphase subphase = Phases.StartTemporarySubPhaseNew<KuiilCritDecisonSubphase>(
                "Kuiil crit repair decision",
                nextPhase
            );

            subphase.DescriptionShort = HostUpgrade.UpgradeInfo.Name;
            subphase.DescriptionLong = "You may repair one faceup Ship damage card.";
            subphase.ImageSource = HostUpgrade;
            subphase.HostShip = HostShip;
            subphase.DecisionOwner = HostShip.Owner;
            subphase.Start();
        }
        private void ChooseRemoveOrangeTokens(Action callBack)
        {
            Action nextPhase = null;
            if (blankIndex == 1)
            {
                nextPhase = () => CheckFocuses(callBack);
            }
            else
            {
                blankIndex--;
                nextPhase = () => ChooseRemoveOrangeTokens(callBack);
            }

            KuiilBlankDecisonSubphase subphase = Phases.StartTemporarySubPhaseNew<KuiilBlankDecisonSubphase>(
                "Kuiil remove orange token decision",
                nextPhase
            );

            subphase.DescriptionShort = HostUpgrade.UpgradeInfo.Name;
            subphase.DescriptionLong = "You may remove one orange token.";
            subphase.ImageSource = HostUpgrade;
            subphase.HostShip = HostShip;
            subphase.DecisionOwner = HostShip.Owner;
            subphase.Start();
        }
        private class KuiilCritDecisonSubphase : DecisionSubPhase
        {
            public GenericShip HostShip { get; set; }
            public override void PrepareDecision(System.Action callBack)
            {
                DecisionViewType = DecisionViewTypes.ImagesDamageCard;

                foreach (GenericDamageCard faceupCrit in HostShip.Damage.GetFaceupCrits(CriticalCardType.Ship))
                {
                    AddDecision(faceupCrit.Name, delegate { Repair(faceupCrit); }, faceupCrit.ImageUrl);
                }
                DefaultDecisionName = GetDecisions().First().Name;
                callBack();
            }
            private void Repair(GenericDamageCard critCard)
            {
                HostShip.Damage.FlipFaceupCritFacedown(critCard, DecisionSubPhase.ConfirmDecision);
            }
        }

        private class KuiilBlankDecisonSubphase : DecisionSubPhase
        {
            public GenericShip HostShip { get; set; }
            public override void PrepareDecision(System.Action callBack)
            {
                DecisionViewType = DecisionViewTypes.TextButtons;

                foreach (GenericToken token in HostShip.Tokens.GetTokensByColor(TokenColors.Orange))
                {
                    if (!GetDecisions().Any(n => n.Name == GetRemoveTokenDescription(token)))
                    {
                        AddDecision(
                            GetRemoveTokenDescription(token),
                            delegate { RemoveToken(token); }
                        );
                    }
                }
                DefaultDecisionName = GetDecisions().First().Name;
                callBack();
            }
            private string GetRemoveTokenDescription(GenericToken token)
            {
                return $"Remove {token.Name}";
            }
            private void RemoveToken(GenericToken token)
            {
                HostShip.Tokens.RemoveToken(token, DecisionSubPhase.ConfirmDecision);
            }
        }
    }
}
