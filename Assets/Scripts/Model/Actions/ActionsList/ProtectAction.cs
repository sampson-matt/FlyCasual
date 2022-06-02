using RulesList;
using Ship;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tokens;
using Editions;
using ActionsList;
using Actions;
using SubPhases;

namespace ActionsList
{

    public class ProtectAction : GenericAction
    {
        public ProtectAction()
        {
            Name = DiceModificationName = "Protect";
        }

        public override void ActionTake()
        {
            ProtectTargetSubPhase protectPhase = Phases.StartTemporarySubPhaseNew<ProtectTargetSubPhase>(
                "Select target to Protect",
                Phases.CurrentSubPhase.CallBack
            );
            protectPhase.HostAction = this;
            protectPhase.Start();
        }

        public override void RevertActionOnFail(bool hasSecondChance = false)
        {
            if (hasSecondChance)
            {
                UI.ShowSkipButton();
                UI.HighlightSkipButton();
            }
            else
            {
                Phases.GoBack();
            }
        }

    }

}

namespace SubPhases
{

    public class ProtectTargetSubPhase : SelectShipSubPhase
    {
        public override void Prepare()
        {
            PrepareByParameters(
                SelectProtectTarget,
                FilterProtectTargets,
                GetAiProtectPriority,
                Selection.ThisShip.Owner.PlayerNo,
                false,
                "Protect Action",
                "Select a friendly ship to get an Evade token."
            );
        }

        protected override void CancelShipSelection()
        {
            Rules.Actions.ActionIsFailed(TheShip, HostAction, ActionFailReason.WrongRange, true);
        }

        public override void SkipButton()
        {
            Rules.Actions.ActionIsFailed(TheShip, HostAction, ActionFailReason.WrongRange, false);
        }

        private int GetAiProtectPriority(GenericShip ship)
        {
            int result = 0;

            return result;
        }

        private bool FilterProtectTargets(GenericShip ship)
        {
            bool result = false;
            if (Rules.Protect.ProtectIsAllowed(Selection.ThisShip, ship)) result = true;
            return result;
        }

        private void SelectProtectTarget()
        {
            Selection.ThisShip.CallProtectTargetIsSelected(TargetShip, PerformProtectEffect);
        }

        private void PerformProtectEffect()
        {
            var protectingShip = Selection.ThisShip;
            var targetShip = TargetShip;
                        
            Triggers.RegisterTrigger(
                new Trigger()
                {
                    Name = "Protect",
                    TriggerOwner = Selection.ThisShip.Owner.PlayerNo,
                    TriggerType = TriggerTypes.OnTokenIsAssigned,
                    EventHandler = (s,e)=>AssignProtectToken(targetShip)
                }
            );

            MovementTemplates.ReturnRangeRuler();

            Triggers.ResolveTriggers(TriggerTypes.OnTokenIsAssigned, delegate {
                Selection.ThisShip = protectingShip;
                Phases.FinishSubPhase(typeof(ProtectTargetSubPhase));
                CallBack();
            });
        }

        private void AssignProtectToken(GenericShip targetShip)
        {
            targetShip.Tokens.AssignToken(new EvadeToken(targetShip), Triggers.FinishTrigger);
        }

        public override void RevertSubPhase()
        {

        }
    }

}
