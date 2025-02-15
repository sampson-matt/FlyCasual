﻿using ActionsList;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SubPhases
{

    public class ActionSubPhase : GenericSubPhase
    {

        public override void Start()
        {
            base.Start();

            Name = "Action SubPhase";
            RequiredInitiative = PreviousSubPhase.RequiredInitiative;
            RequiredPlayer = PreviousSubPhase.RequiredPlayer;
            CanBePaused = true;
            UpdateHelpInfo();
        }

        public override void Initialize()
        {
            Selection.ThisShip.CallPerformActionStepStart();
            Phases.Events.CallBeforeActionSubPhaseTrigger();
            var ship = Selection.ThisShip;

            if (RulesList.ActionsRule.HasPerformActionStep(ship))
            {
                ship.GenerateAvailableActionsList();
                Triggers.RegisterTrigger(
                    new Trigger() {
                        Name = "Action",
                        TriggerOwner = Phases.CurrentPhasePlayer,
                        TriggerType = TriggerTypes.OnActionSubPhaseStart,
                        EventHandler = StartActionDecisionSubphase
                    }
                );

                Phases.Events.CallOnActionSubPhaseTrigger();
            }
            else
            {
                ship.CallMovementActivationFinish(Phases.Events.CallOnActionSubPhaseTrigger);
            }
        }

        private void StartActionDecisionSubphase(object sender, System.EventArgs e)
        {
            Phases.StartTemporarySubPhaseOld(
                "Action Decision",
                typeof(ActionDecisonSubPhase),
                (Action)delegate {
                    ActionsHolder.TakeActionFinish(
                        delegate {
                            ActionsHolder.EndActionDecisionSubhase(Finish);
                        }
                    ); 
                }
            );
        }

        private void Finish()
        {
            UI.HideSkipButton();

            Phases.FinishSubPhase(typeof(ActionDecisonSubPhase));

            Selection.ThisShip.CallMovementActivationFinish(
                delegate {
                    (Phases.CurrentPhase as MainPhases.ActivationPhase).ActivationShip = null;
                    Triggers.FinishTrigger();
                }
            );
        }

        public override void Next()
        {
            FinishPhase();
        }

        public override void FinishPhase()
        {
            GenericSubPhase activationSubPhase = new ActivationSubPhase();
            Phases.CurrentSubPhase = activationSubPhase;
            Phases.CurrentSubPhase.Start();
            Phases.CurrentSubPhase.RequiredInitiative = RequiredInitiative;
            Phases.CurrentSubPhase.RequiredPlayer = RequiredPlayer;

            Phases.CurrentSubPhase.Next();
        }

        public override bool ThisShipCanBeSelected(Ship.GenericShip ship, int mouseKeyIsPressed)
        {
            bool result = false;
            Messages.ShowErrorToHuman(ship.PilotName + " cannot be selected, perform an action first");
            return result;
        }

    }

}

namespace SubPhases
{

    public class ActionDecisonSubPhase : DecisionSubPhase
    {
        public bool ActionWasPerformed { get; private set; }

        public override void PrepareDecision(System.Action callBack)
        {
            decisions.Clear();
            DescriptionShort = "Perform Action step";

            ShowSkipButton = true;
            DefaultDecisionName = "Focus";

            List<GenericAction> availableActions = Selection.ThisShip.GetAvailableActions();

            if (availableActions.Count > 0)
            {
                GenerateActionButtons();
                callBack();
            }
            else
            {
                if (!DecisionWasPreparedAndShown)
                {
                    Messages.ShowErrorToHuman("This ship cannot perform any actions");
                    ActionsHolder.CurrentAction = null;
                    CallBack();
                }
            }
        }

        public void GenerateActionButtons()
        {
            //TODO: Use more global way of fix
            HideDecisionWindowUI();

            List<GenericAction> availableActions = Selection.ThisShip.GetAvailableActions();
            foreach (var action in availableActions)
            {
                bool addedDecisionWithLink = false;

                string decisionName = GetActionNameColored(action);

                foreach (var kv in Selection.ThisShip.ActionBar.LinkedActions)
                {
                    Type actionType = kv.Key;
                    GenericAction linkedAction = kv.Value;

                    if (action.GetType() == actionType)
                    {
                        string linkedActionName = GetActionNameColored(kv.Value);

                        if (!addedDecisionWithLink)
                        {
                            decisionName += " > " + linkedActionName;
                            addedDecisionWithLink = true;
                        }
                        else
                        {
                            decisionName += " / " + linkedActionName;
                        }
                    }
                }

                AddDecision(
                    decisionName,
                    delegate {
                        ActionWasPerformed = true;
                        Selection.ThisShip.CallBeforeActionIsPerformed(
                            (GenericAction)action,
                            (Action)delegate { ActionsHolder.TakeActionStart((GenericAction)action); },
                            isFree: false
                        );
                    },
                    action.ImageUrl
                );
            }
        }

        private string GetActionNameColored(GenericAction action)
        {
            string actionName = action.Name;

            Actions.ActionColor actionColor = action.Color;
            Selection.ThisShip.CallOnCheckActionComplexity(action, ref actionColor);

            switch (actionColor)
            {
                case Actions.ActionColor.Red:
                    actionName = "<color=red>" + actionName + "</color>";
                    break;
                case Actions.ActionColor.Purple:
                    actionName = "<color=purple>" + actionName + "</color>";
                    break;
                default:
                    break;
            }

            return actionName;
        }

        public override void Resume()
        {
            base.Resume();

            UI.ShowSkipButton();
        }

        public override void SkipButton()
        {
            ActionsHolder.CurrentAction = null;
            CallBack();
        }

    }

}

namespace SubPhases
{

    public class FreeActionDecisonSubPhase : DecisionSubPhase
    {
        public bool ActionWasPerformed { get; private set; }

        public override void PrepareDecision(System.Action callBack)
        {
            DescriptionShort = DescriptionShort ?? "Select free action";
            DefaultDecisionName = "Focus";

            List<GenericAction> availableActions = Selection.ThisShip.GetAvailableFreeActions();

            if (availableActions.Count > 0)
            {
                GenerateFreeActionButtons();
                callBack();
            }
            else
            {
                Messages.ShowErrorToHuman(Selection.ThisShip.PilotInfo.PilotName + " cannot perform any free actions");
                Selection.ThisShip.IsFreeActionSkipped = true;
                ActionsHolder.CurrentAction = null;
                CallBack();
            }
        }

        public void GenerateFreeActionButtons()
		{
			Selection.ThisShip.IsFreeActionSkipped = false;
            List<GenericAction> availableActions = Selection.ThisShip.GetAvailableFreeActions();

            foreach (var action in availableActions)
            {
                bool addedDecisionWithLink = false;

                string decisionName = GetActionNameColored(action);

                foreach (var kv in Selection.ThisShip.ActionBar.LinkedActions)
                {
                    Type actionType = kv.Key;
                    GenericAction linkedAction = kv.Value;

                    if (action.GetType() == actionType)
                    {
                        string linkedActionName = GetActionNameColored(kv.Value);

                        if (!addedDecisionWithLink)
                        {
                            decisionName += " > " + linkedActionName;
                            addedDecisionWithLink = true;
                        }
                        else
                        {
                            decisionName += " / " + linkedActionName;
                        }
                    }
                }

                AddDecision(
                    decisionName,
                    delegate {
                        ActionWasPerformed = true;
                        Selection.ThisShip.CallBeforeActionIsPerformed(
                            (GenericAction)action,
                            (Action)delegate { ActionsHolder.TakeActionStart((GenericAction)action); },
                            isFree: false
                        );
                    },
                    action.ImageUrl
                );
            }
        }

        private string GetActionNameColored(GenericAction action)
        {
            string actionName = action.Name;

            Actions.ActionColor actionColor = action.Color;
            Selection.ThisShip.CallOnCheckActionComplexity(action, ref actionColor);

            switch (actionColor)
            {
                case Actions.ActionColor.Red:
                    actionName = "<color=red>" + actionName + "</color>";
                    break;
                case Actions.ActionColor.Purple:
                    actionName = "<color=purple>" + actionName + "</color>";
                    break;
                default:
                    break;
            }

            return actionName;
        }

        public override void Resume()
        {
            base.Resume();

            if (ShowSkipButton) UI.ShowSkipButton(); else UI.HideSkipButton();
        }

        public override void SkipButton()
        {
            UI.HideSkipButton();
            ActionsHolder.CurrentAction = null;
            Selection.ThisShip.IsFreeActionSkipped = true;
            CallBack();
        }

    }

}
