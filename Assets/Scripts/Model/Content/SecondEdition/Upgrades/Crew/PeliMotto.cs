using Ship;
using Upgrade;
using System.Linq;
using System.Collections.Generic;
using System;
using ActionsList;
using SubPhases;

namespace UpgradesList.SecondEdition
{
    public class PeliMotto : GenericUpgrade
    {
        public PeliMotto() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Peli Motto",
                UpgradeType.Crew,
                cost: 3,
                isLimited: true,
                restrictions: new UpgradeCardRestrictions(
                    new FactionRestriction(Faction.Scum),
                    new BaseSizeRestriction(BaseSize.Medium, BaseSize.Large)
                ),
                abilityType: typeof(Abilities.SecondEdition.PeliMottoCrewAbility)
            );
        }
    }
}
namespace Abilities.SecondEdition
{
    //During the System Phase, you may perform an action on 1 of your damage cards, even while stressed. 
    //After you repair a faceup Ship damage card, you may roll 1 attack die. On a hit result, repair another faceup Ship damage card. On a crit result, expose 1 damage card.
    public class PeliMottoCrewAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnCheckSystemsAbilityActivation += CheckAbility;
            HostShip.OnSystemsAbilityActivation += RegisterAbility;
            HostShip.OnFaceupDamageCardIsRepaired += CheckSecondRepair;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnCheckSystemsAbilityActivation -= CheckAbility;
            HostShip.OnSystemsAbilityActivation -= RegisterAbility;
            HostShip.OnFaceupDamageCardIsRepaired -= CheckSecondRepair;
        }

        private void CheckSecondRepair(GenericDamageCard damageCard)
        {
            if (damageCard.Type == CriticalCardType.Ship && HostShip.Damage.DamageCards.Where(d => d.IsFaceup && d.Type==CriticalCardType.Ship).Count()>0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnFaceupDamageCardIsRepaired, AskToRepairAgain);
            }
        }

        private void AskToRepairAgain(object sender, EventArgs e)
        {
            AskToUseAbility(HostUpgrade.UpgradeInfo.Name,
                AlwaysUseByDefault,
                DiceCheck,
                descriptionLong: "Do you want to roll 1 attack die? On a hit result, repair another faceup Ship damage card. On a crit result, expose 1 damage card.",
                imageHolder: HostUpgrade,
                requiredPlayer: HostShip.Owner.PlayerNo
            );

            
        }

        private void DiceCheck(object sender, EventArgs e)
        {
            PerformDiceCheck(
                HostName,
                DiceKind.Attack,
                1,
                DiceCheckFinished,
                SubPhases.DecisionSubPhase.ConfirmDecision);
        }

        private void DiceCheckFinished()
        {
            if (DiceCheckRoll.CriticalSuccesses > 0)
            {
                Messages.ShowInfo(HostName + " exposes one damage card");
                HostShip.Damage.ExposeRandomFacedownCard(AbilityDiceCheck.ConfirmCheck);
            }
            if (DiceCheckRoll.Successes > 0)
            {

                var phase = Phases.StartTemporarySubPhaseNew<PeliMottoDecisionSubPhase>(
                "Peli Motto: You may roll 1 attack die. On a hit result, repair another faceup Ship damage card. On a crit result, expose 1 damage card.",
                AbilityDiceCheck.ConfirmCheck
                );

                phase.DescriptionShort = "Peli Motto";
                phase.DescriptionLong = "Select a faceup damage card to repair.";
                phase.ImageSource = HostUpgrade;

                phase.HostShip = HostShip;
                phase.DecisionOwner = HostShip.Owner;
                phase.ShowSkipButton = true;

                phase.Start();
            }
            else
            {
                AbilityDiceCheck.ConfirmCheck();
            }
        }

        private class PeliMottoDecisionSubPhase : DecisionSubPhase
        {
            public GenericShip HostShip { get; set; }

            public override void PrepareDecision(Action callBack)
            {
                DecisionViewType = DecisionViewTypes.ImagesDamageCard;

                if (HostShip.Damage.HasFaceupCards)
                {
                    foreach (var crit in HostShip.Damage.GetFaceupCrits().Where(c => c.Type == CriticalCardType.Ship).ToList())
                    {
                        AddDecision(crit.Name, delegate { DiscardCrit(crit); }, crit.ImageUrl);
                    }

                    DefaultDecisionName = GetDecisions().First().Name;
                }

                callBack();
            }

            private void DiscardCrit(GenericDamageCard critCard)
            {
                Selection.ActiveShip.Damage.FlipFaceupCritFacedown(critCard, Phases.CurrentSubPhase.CallBack);
                //ConfirmDecision();
                
            }

        }

        private void CheckAbility(GenericShip ship, ref bool flag)
        {
            if (HostShip.Damage.HasFaceupCards)
            {
                flag = true;
            }
            
        }

        private List<GenericAction> getCritCancelActions()
        {
            return HostShip.GetAvailableActions().Where(a => a.IsCritCancelAction).ToList();
        }

        private void RegisterAbility(GenericShip ship)
        {
            if (HostShip.Damage.HasFaceupCards)
            {
                RegisterAbilityTrigger(TriggerTypes.OnSystemsAbilityActivation, AskToPerformRepairAction);
            }
        }

        private void AskToPerformRepairAction(object sender, EventArgs e)
        {
            HostShip.OnCanPerformActionWhileStressed += TemporaryAllowAnyActionsWhileStressed;
            HostShip.OnCheckCanPerformActionsWhileStressed += TemporaryAllowActionsWhileStressed;

            HostShip.AskPerformFreeAction
            (
                getCritCancelActions(),
                RevertActionCanBePerformedWhileStressed,
                descriptionShort: HostUpgrade.UpgradeInfo.Name,
                descriptionLong: "You may perform one of these actions.",
                imageHolder: HostUpgrade
            );
        }

        private void TemporaryAllowAnyActionsWhileStressed(GenericAction action, ref bool isAllowed)
        {
            isAllowed = true;
        }

        private void TemporaryAllowActionsWhileStressed(ref bool isAllowed)
        {
            isAllowed = true;
        }

        private void RevertActionCanBePerformedWhileStressed()
        {
            HostShip.OnCanPerformActionWhileStressed -= TemporaryAllowAnyActionsWhileStressed;
            HostShip.OnCheckCanPerformActionsWhileStressed -= TemporaryAllowActionsWhileStressed;
            Triggers.FinishTrigger();
        }
    }
}