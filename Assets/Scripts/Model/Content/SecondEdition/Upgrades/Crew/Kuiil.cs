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
            IsWIP = true;

            UpgradeInfo = new UpgradeCardInfo(
                "Kuiil",
                UpgradeType.Crew,
                cost: 6,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Scum),
                abilityType: typeof(Abilities.SecondEdition.KuiilAbility)
            );

            ImageUrl = "https://infinitearenas.com/xw2/images/upgrades/kuiil.png";
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
        public List<GenericToken> orangeTokens = new List<GenericToken>();

        public int hitIndex = 0;

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
                DiceCheckFinished,
                Phases.CurrentSubPhase.CallBack
            );
        }
        protected void DiceCheckFinished()
        {
            if (DiceCheckRoll.Successes > 0 && HostShip.Damage.GetFaceupCrits(CriticalCardType.Ship).Any())
            {
                if (DiceCheckRoll.Successes >= HostShip.Damage.GetFaceupCrits(CriticalCardType.Ship).Count)
                {
                    foreach(GenericDamageCard card in HostShip.Damage.GetFaceupCrits(CriticalCardType.Ship))
                    {
                        HostShip.Damage.FlipFaceupCritFacedown(card, delegate { });
                        Messages.ShowInfo(card.Name + " repaired");
                    }
                }
                else
                {
                    hitIndex = DiceCheckRoll.Successes;
                    ChooseRepairFaceupCrits();
                }
            }
            if (DiceCheckRoll.CriticalSuccesses > 0 && HostShip.Damage.HasFacedownCards)
            {
                for (int i = 0; i < DiceCheckRoll.CriticalSuccesses; i++)
                {
                    HostShip.Damage.DiscardRandomFacedownCard();
                    Messages.ShowInfo("Facedown damage card repaired.");
                }
            }
            if (DiceCheckRoll.Blanks > 0 && HostShip.Tokens.CountTokensByColor(TokenColors.Orange) > 0)
            {
                if(HostShip.Tokens.CountTokensByColor(TokenColors.Orange) <= DiceCheckRoll.Blanks)
                {
                    HostShip.Tokens.RemoveAllTokensByColor(TokenColors.Orange, delegate { });
                    Messages.ShowInfo("Orange Tokens Removed");
                }

            }
            if (DiceCheckRoll.Focuses > 0)
            {
                HostShip.Tokens.AssignTokens(CreateFocusToken, DiceCheckRoll.Focuses, delegate { });
            }
            AbilityDiceCheck.ConfirmCheck();
        }

        private void ChooseRepairFaceupCrits()
        {
            KuiilCritDecisonSubphase subphase = Phases.StartTemporarySubPhaseNew<KuiilCritDecisonSubphase>(
                "Kuiil crit repair decision",
                Phases.CurrentSubPhase.CallBack
            );

            subphase.DescriptionShort = HostShip.PilotInfo.PilotName;
            subphase.DescriptionLong = "You may repair one faceup Ship damage card.";
            subphase.ImageSource = HostUpgrade;

            foreach (GenericDamageCard faceupCrit in HostShip.Damage.GetFaceupCrits(CriticalCardType.Ship))
            {
                subphase.AddDecision(faceupCrit.Name, delegate { Repair(faceupCrit); }, faceupCrit.ImageUrl);
            }
            subphase.DefaultDecisionName = subphase.GetDecisions().First().Name;


            subphase.DecisionOwner = HostShip.Owner;

            subphase.Start();
        }

        private void Repair(GenericDamageCard critCard)
        {
            HostShip.Damage.FlipFaceupCritFacedown(critCard, DecisionSubPhase.ConfirmDecision);
           // DecisionSubPhase.ConfirmDecision();
            //hitIndex--;
            //if(hitIndex >0)
            //{
            //    ChooseRepairFaceupCrits();
            //}
        }

        private GenericToken CreateFocusToken()
        {
            return new Tokens.FocusToken(HostShip);
        }

        private class KuiilCritDecisonSubphase : DecisionSubPhase { }


    }
}
