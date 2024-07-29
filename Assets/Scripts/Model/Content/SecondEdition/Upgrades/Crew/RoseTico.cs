using Ship;
using Upgrade;
using ActionsList;
using UnityEngine;
using Players;
using SubPhases;
using System.Collections.Generic;

namespace UpgradesList.SecondEdition
{
    public class RoseTico : GenericUpgrade
    {
        public RoseTico() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Rose Tico",
                UpgradeType.Crew,
                cost: 9,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Resistance),
                abilityType: typeof(Abilities.SecondEdition.RoseTicoAbility)
            );

            Avatar = new AvatarInfo(
                Faction.Resistance,
                new Vector2(301, 3)
            );
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class RoseTicoAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnGenerateDiceModifications += AddRoseTicoDiceModification;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnGenerateDiceModifications -= AddRoseTicoDiceModification;
        }

        private void AddRoseTicoDiceModification(GenericShip host)
        {
            GenericAction diceModification = new ActionsList.SecondEdition.RoseTicoDiceModification()
            {
                HostShip = host,
                ImageUrl = HostUpgrade.ImageUrl
            };
            host.AddAvailableDiceModificationOwn(diceModification);
        }
    }
}

namespace ActionsList.SecondEdition
{
    public class RoseTicoDiceModification : GenericAction
    {
        public RoseTicoDiceModification()
        {
            Name = DiceModificationName = "Rose Tico's Ability";
        }

        public override bool IsDiceModificationAvailable()
        {
            return Combat.CurrentDiceRoll.Count > 0;
        }

        public override int GetDiceModificationPriority()
        {
            // TODO: Improve AI

            int result = 0;

            GenericShip opponentShip = (Combat.AttackStep == CombatStep.Attack) ? Combat.Defender : Combat.Attacker;

            if (Combat.CurrentDiceRoll.WorstResult == DieSide.Blank || Combat.CurrentDiceRoll.WorstResult == DieSide.Focus
                && !ActionsHolder.HasTargetLockOn(HostShip, opponentShip))
            {
                result = 1;
            }

            return result;
        }

        public override void ActionEffect(System.Action callback)
        {
            if (HostShip.Owner is HumanPlayer)
            {
                Triggers.RegisterTrigger(
                    new Trigger()
                    {
                        Name = DiceModificationName,
                        TriggerOwner = HostShip.Owner.PlayerNo,
                        TriggerType = TriggerTypes.OnAbilityDirect,
                        EventHandler = StartSubphase
                    }
                );

                Triggers.ResolveTriggers(TriggerTypes.OnAbilityDirect, callback);
            }
            else
            {
                DieSide worstDieSide = Combat.CurrentDiceRoll.WorstResult;
                Combat.CurrentDiceRoll.RemoveType(worstDieSide);
                GenericShip opponentShip = (Combat.AttackStep == CombatStep.Attack) ? Combat.Defender : Combat.Attacker;
                ActionsHolder.AcquireTargetLock(HostShip, opponentShip, callback, callback);
            }
        }

        private void StartSubphase(object sender, System.EventArgs e)
        {
            var spendDiceSubPhase = Phases.StartTemporarySubPhaseNew<RoseTicoDecisionSubPhase>(Name, Triggers.FinishTrigger);
            spendDiceSubPhase.HostShip = HostShip;
            spendDiceSubPhase.ShowSkipButton = true;
            spendDiceSubPhase.OnSkipButtonIsPressed = DecisionSubPhase.ConfirmDecision;
            spendDiceSubPhase.DecisionOwner = HostShip.Owner;
            spendDiceSubPhase.Start();
        }
    }
}

namespace SubPhases
{
    public class RoseTicoDecisionSubPhase : SpendDiceResultDecisionSubPhase
    {
        public GenericShip HostShip;

        protected override void PrepareDiceResultEffects()
        {
            DescriptionShort = "Rose Tico";
            DescriptionLong = "Select a die result to spend to acquire a lock";
            ImageSource = HostShip;

            if(Combat.AttackStep == CombatStep.Attack)
            {
                AddSpendDiceResultEffect(DieSide.Crit, "Critical result", delegate { SpendResultToLock(DieSide.Crit); });
                AddSpendDiceResultEffect(DieSide.Success, "Hit result", delegate { SpendResultToLock(DieSide.Success); });
                AddSpendDiceResultEffect(DieSide.Focus, "Focus result", delegate { SpendResultToLock(DieSide.Focus); });
                AddSpendDiceResultEffect(DieSide.Blank, "Blank result", delegate { SpendResultToLock(DieSide.Blank); });
            } 
            else
            {
                AddSpendDiceResultEffect(DieSide.Success, "Evade result", delegate { SpendResultToLock(DieSide.Success); });
                AddSpendDiceResultEffect(DieSide.Focus, "Focus result", delegate { SpendResultToLock(DieSide.Focus); });
                AddSpendDiceResultEffect(DieSide.Blank, "Blank result", delegate { SpendResultToLock(DieSide.Blank); });
            }
            
        }

        private void SpendResultToLock(DieSide side)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();
            if (Combat.AttackStep == CombatStep.Attack)
            {
                Combat.DiceRollAttack.RemoveType(side);
                Combat.DiceRollAttack.OrganizeDicePositions();
            }
            else
            {
                Combat.DiceRollDefence.RemoveType(side);
                Combat.DiceRollDefence.OrganizeDicePositions();
            }

            GenericShip opponentShip = (Combat.AttackStep == CombatStep.Attack) ? Combat.Defender : Combat.Attacker;
            ActionsHolder.AcquireTargetLock(HostShip, opponentShip, Triggers.FinishTrigger, Triggers.FinishTrigger);
        }
    }
}