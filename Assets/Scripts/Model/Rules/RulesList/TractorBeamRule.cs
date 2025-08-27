using ActionsList;
using BoardTools;
using Editions;
using Players;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Movement;

namespace RulesList
{
    public class TractorBeamRule
    {
        static bool RuleIsInitialized = false;

        public TractorBeamRule ()
        {
            if (!RuleIsInitialized)
            {
                GenericShip.OnTokenIsAssignedGlobal += CheckForTractorBeam;
                GenericShip.OnTokenIsRemovedGlobal += CheckForTractorBeamRemoval;
                RuleIsInitialized = true;
            }
        }

        private void CheckForTractorBeam(GenericShip ship, GenericToken token)
        {
            if (!(token is TractorBeamToken)) return;

            if (ShouldDecreaseAgility(ship)) ship.ChangeAgilityBy(-1);

            if (IsTractorBeamReposition(ship))
            {
                (token as TractorBeamToken).Assigner.PerformTractorBeamReposition(ship);
            }
        }

        public static bool IsTractorBeamReposition(GenericShip ship)
        {
            int tractorBeamTokensCount = ship.Tokens.GetAllTokens().Count(n => n is TractorBeamToken);
            return (tractorBeamTokensCount == Edition.Current.NegativeTokensToAffectShip[ship.ShipInfo.BaseSize]);
        }

        private bool ShouldDecreaseAgility(GenericShip ship)
        {
            int tractorBeamTokensCount = ship.Tokens.GetAllTokens().Count(n => n is TractorBeamToken);
            
            if (Edition.Current is SecondEdition)
            {
                // only decrease agility after gaining a token which takes us to the exact NegativeTokensToAffectShip value.
                // gaining additional tokens beyond this value should not decrease agility
                return (tractorBeamTokensCount == Edition.Current.NegativeTokensToAffectShip[ship.ShipInfo.BaseSize]);
            }

            return (tractorBeamTokensCount >= Edition.Current.NegativeTokensToAffectShip[ship.ShipInfo.BaseSize]);
        }

        public static void PerfromManualTractorBeamReposition(GenericShip ship, GenericPlayer assinger)
        {
            SubPhases.TractorBeamPlanningSubPhase newPhase = (SubPhases.TractorBeamPlanningSubPhase)Phases.StartTemporarySubPhaseNew(
                "Perform tractor beam effect",
                typeof(SubPhases.TractorBeamPlanningSubPhase),
                Triggers.FinishTrigger
            );
            newPhase.Assigner = assinger;
            newPhase.TheShip = ship;

            Triggers.RegisterTrigger(new Trigger()
            {
                Name = "Perform tractor beam",
                TriggerType = TriggerTypes.OnTokenIsAssigned,
                TriggerOwner = assinger.PlayerNo,
                EventHandler = delegate {
                    newPhase.Start();
                }
            });
        }

        private void CheckForTractorBeamRemoval(GenericShip ship, GenericToken token)
        {
            if (!(token is TractorBeamToken)) return;

            if (ShouldIncreaseAgility(ship)) ship.ChangeAgilityBy(+1);
        }

        private bool ShouldIncreaseAgility(GenericShip ship)
        {
            bool result = true;

            if (Edition.Current is SecondEdition)
            {
                // only increase agility after losing a token which takes us to one less than the NegativeTokensToAffectShip value
                // losing additional tokens beyond this should not increase agility
                int tractorBeamTokensCount = ship.Tokens.CountTokensByType(typeof(TractorBeamToken));   
                return (tractorBeamTokensCount + 1 == Edition.Current.NegativeTokensToAffectShip[ship.ShipInfo.BaseSize]);
            }

            return result;
        }
    }
}

namespace SubPhases
{
    public class TractorBeamPlanningSubPhase : GenericSubPhase
    {
        public GenericPlayer Assigner;
        private Action selectedPlanningAction;
        private bool canBoost = true;
        private BarrelRollAction stubAction;
        protected List<ManeuverTemplate> AvailableRepositionTemplates = new List<ManeuverTemplate>();

        public override void Start()
        {
            Name = "Tractor Beam planning";
            IsTemporary = true;
            UpdateHelpInfo();

            CheckCanBoost();
        }

        private void InitializeBoostPlanning(BoostPlanningSubPhase boostPlanning)
        {
            boostPlanning.TheShip = TheShip;
            boostPlanning.Name = "Tractor beam boost";
            boostPlanning.IsTemporary = true;
            boostPlanning.SelectedBoostHelper = "Straight 1";
            boostPlanning.IsTractorBeamBoost = true;
            boostPlanning.IsIgnoreObstacles = Edition.Current.RuleSet.AllowTractoringOnObstacle;
            boostPlanning.InitializeRendering();
        }

        private void CheckCanBoost()
        {
            BoostPlanningSubPhase boostPlanning = new BoostPlanningSubPhase ();
            InitializeBoostPlanning(boostPlanning);
            boostPlanning.TryConfirmBoostPosition(CheckCanBoostCallback);
        }

        private void CheckCanBoostCallback(bool canBoostResult)
        {
            this.canBoost = canBoostResult;
            RegisterTractorPlanning();
        }

        public void RegisterTractorPlanning()
        {
            Triggers.RegisterTrigger(new Trigger()
            {
                Name = "Select tractor beam direction",
                TriggerType = TriggerTypes.OnAbilityDirect,
                TriggerOwner = Assigner.PlayerNo,
                EventHandler = delegate {
                    StartSelectTemplateSubphase();
                }
            });

            Triggers.ResolveTriggers(TriggerTypes.OnAbilityDirect, ExecutePlanning);
        }

        private void ExecutePlanning()
        {
            if (selectedPlanningAction != null)
            {
                selectedPlanningAction();
            }
            else
            {
                Next();
            }
        }

        private void PerfromBrTemplatePlanning(ManeuverTemplate template, Direction direction, Direction directionSecondary = Direction.None)
        {
            BarrelRollPlanningSubPhase brPlanning = (BarrelRollPlanningSubPhase) Phases.StartTemporarySubPhaseNew(
                "Select position",
                typeof(BarrelRollPlanningSubPhase),
                delegate {
                    FinishTractorBeamMovement();
                }
            );
            brPlanning.Name = "Select position";
            brPlanning.TheShip = TheShip;
            brPlanning.IsTemporary = true;
            brPlanning.Controller = Assigner;
            brPlanning.HostAction = stubAction;

            brPlanning.IsTractorBeamBarrelRoll = true;
            brPlanning.IsIgnoreObstacles = Edition.Current.RuleSet.AllowTractoringOnObstacle;
            brPlanning.SelectTemplate(
                template,
                direction,
                directionSecondary
            );

            Phases.UpdateHelpInfo();
            brPlanning.PerfromTemplatePlanning();
        }

        //private void PerfromLeftBrTemplatePlanning()
        //{
        //    PerfromBrTemplatePlanning(Direction.Left);
        //}

        //private void PerfromRightBrTemplatePlanning()
        //{
        //    PerfromBrTemplatePlanning(Direction.Right);
        //}

        private void PerfromStraightTemplatePlanning()
        {
            BoostAction boostAction = new BoostAction() { HostShip = TheShip };

            BoostPlanningSubPhase boostPlanning = (BoostPlanningSubPhase) Phases.StartTemporarySubPhaseNew(
                "Boost",
                typeof(BoostPlanningSubPhase),
                delegate {
                    FinishTractorBeamMovement();
                }
            );
            boostPlanning.HostAction = boostAction;
            InitializeBoostPlanning(boostPlanning);
            Phases.UpdateHelpInfo();
            boostPlanning.TryConfirmBoostPosition();
        }

        private void StartSelectTemplateSubphase()
        {
            selectedPlanningAction = null;

            stubAction = new BarrelRollAction { HostShip = TheShip };

            List<ManeuverTemplate> allowedTemplates = TheShip.GetAvailableBarrelRollTemplates(stubAction);

            foreach (ManeuverTemplate barrelRollTemplate in allowedTemplates)
            {
                AvailableRepositionTemplates.Add(barrelRollTemplate);
            }

            TractorBeamDirectionDecisionSubPhase selectTractorDirection = (TractorBeamDirectionDecisionSubPhase)Phases.StartTemporarySubPhaseNew(
                Name,
                typeof(TractorBeamDirectionDecisionSubPhase),
                Triggers.FinishTrigger
            );

            if (canBoost)
            {
                selectTractorDirection.AddDecision(
                    "Straight",
                    delegate {
                        selectedPlanningAction = PerfromStraightTemplatePlanning;
                        DecisionSubPhase.ConfirmDecision();
                    },
                    isCentered: true
                );
            }

            // Straight templates
            foreach (ManeuverTemplate template in AvailableRepositionTemplates)
            {
                if (template.Bearing == ManeuverBearing.Straight)
                {
                    selectTractorDirection.AddDecision(
                        "Left " + template.NameNoDirection,
                        delegate {
                            selectedPlanningAction = () => PerfromBrTemplatePlanning(template, Direction.Left);
                            DecisionSubPhase.ConfirmDecision();
                        }
                    );

                    selectTractorDirection.AddDecision(
                        "Right " + template.NameNoDirection,
                        (EventHandler)delegate {
                            selectedPlanningAction = () => PerfromBrTemplatePlanning(template, Direction.Right);
                            DecisionSubPhase.ConfirmDecision();
                        }
                    );
                }
            }

            // Bank templates
            ManeuverTemplate bankLeft = AvailableRepositionTemplates.FirstOrDefault(n => n.Bearing == ManeuverBearing.Bank && n.Direction == ManeuverDirection.Left);
            ManeuverTemplate bankRight = AvailableRepositionTemplates.FirstOrDefault(n => n.Bearing == ManeuverBearing.Bank && n.Direction == ManeuverDirection.Right);

            if (bankLeft != null && bankRight != null)
            {
                selectTractorDirection.AddDecision(
                    "Left " + bankRight.NameNoDirection + " Forward",
                    (EventHandler)delegate
                    {
                        selectedPlanningAction = () => PerfromBrTemplatePlanning(bankRight, Direction.Left, Direction.Top);
                        DecisionSubPhase.ConfirmDecision();
                    }
                );

                selectTractorDirection.AddDecision(
                    "Right " + bankLeft.NameNoDirection + " Forward",
                    (EventHandler)delegate
                    {
                        selectedPlanningAction = () => PerfromBrTemplatePlanning(bankLeft, Direction.Right, Direction.Top);
                        DecisionSubPhase.ConfirmDecision();
                    }
                );

                selectTractorDirection.AddDecision(
                    "Left " + bankLeft.NameNoDirection + " Backwards",
                    (EventHandler)delegate
                    {
                        selectedPlanningAction = () => PerfromBrTemplatePlanning(bankLeft, Direction.Left, Direction.Bottom);
                        DecisionSubPhase.ConfirmDecision();
                    }
                );

                selectTractorDirection.AddDecision(
                    "Right " + bankRight.NameNoDirection + " Backwards",
                    (EventHandler)delegate
                    {
                        selectedPlanningAction = () => PerfromBrTemplatePlanning(bankRight, Direction.Right, Direction.Bottom);
                        DecisionSubPhase.ConfirmDecision();
                    }
                );
            }

            // Bank templates
            ManeuverTemplate turnLeft = AvailableRepositionTemplates.FirstOrDefault(n => n.Bearing == ManeuverBearing.Turn && n.Direction == ManeuverDirection.Left);
            ManeuverTemplate turnRight = AvailableRepositionTemplates.FirstOrDefault(n => n.Bearing == ManeuverBearing.Turn && n.Direction == ManeuverDirection.Right);

            if (turnLeft != null && turnRight != null)
            {
                selectTractorDirection.AddDecision(
                    "Left " + turnRight.NameNoDirection + " Forward",
                    (EventHandler)delegate
                    {
                        selectedPlanningAction = () => PerfromBrTemplatePlanning(turnRight, Direction.Left, Direction.Top);
                        DecisionSubPhase.ConfirmDecision();
                    }
                );

                selectTractorDirection.AddDecision(
                    "Right " + turnLeft.NameNoDirection + " Forward",
                    (EventHandler)delegate
                    {
                        selectedPlanningAction = () => PerfromBrTemplatePlanning(turnLeft, Direction.Right, Direction.Top);
                        DecisionSubPhase.ConfirmDecision();
                    }
                );

                selectTractorDirection.AddDecision(
                    "Left " + turnLeft.NameNoDirection + " Backwards",
                    (EventHandler)delegate
                    {
                        selectedPlanningAction = () => PerfromBrTemplatePlanning(turnLeft, Direction.Left, Direction.Bottom);
                        DecisionSubPhase.ConfirmDecision();
                    }
                );

                selectTractorDirection.AddDecision(
                    "Right " + turnRight.NameNoDirection + " Backwards",
                    (EventHandler)delegate
                    {
                        selectedPlanningAction = () => PerfromBrTemplatePlanning(turnRight, Direction.Right, Direction.Bottom);
                        DecisionSubPhase.ConfirmDecision();
                    }
                );
            }

            //selectTractorDirection.AddDecision("Left", delegate {
            //    selectedPlanningAction = PerfromLeftBrTemplatePlanning;
            //    DecisionSubPhase.ConfirmDecision();
            //});

            //selectTractorDirection.AddDecision("Right", delegate {
            //    selectedPlanningAction = PerfromRightBrTemplatePlanning;
            //    DecisionSubPhase.ConfirmDecision();
            //});

            selectTractorDirection.DescriptionShort = "Tractor beam";
            selectTractorDirection.DescriptionLong = "Select direction for " + TheShip.PilotInfo.PilotName;

            selectTractorDirection.DefaultDecisionName = selectTractorDirection.GetDecisions().First().Name;
            selectTractorDirection.RequiredPlayer = Assigner.PlayerNo;
            selectTractorDirection.ShowSkipButton = true;

            selectTractorDirection.Start();
        }

        private void FinishTractorBeamMovement()
        {
            if (Assigner == TheShip.Owner)
            {
                CheckObstacles();
                return;
            }

            var selectRotateDecision = Phases.StartTemporarySubPhaseNew<DecisionSubPhase>(Name, Triggers.FinishTrigger);

            selectRotateDecision.AddDecision("Left", delegate {
                DecisionSubPhase.ConfirmDecisionNoCallback();
                RotateTractoredShip(Direction.Left, CheckObstacles);
            });

            selectRotateDecision.AddDecision("Right", delegate {
                DecisionSubPhase.ConfirmDecisionNoCallback();
                RotateTractoredShip(Direction.Right, CheckObstacles);
            });

            selectRotateDecision.DescriptionShort = "Tractor beam";
            selectRotateDecision.DescriptionLong = "You may rotate tractored ship 90 degrees";

            selectRotateDecision.DefaultDecisionName = SelectAIRotateDecision(TheShip);
            selectRotateDecision.RequiredPlayer = TheShip.Owner.PlayerNo;
            selectRotateDecision.ShowSkipButton = true;
            selectRotateDecision.OnSkipButtonIsPressed += delegate {
                DecisionSubPhase.ConfirmDecisionNoCallback();
                CheckObstacles();
            };

            selectRotateDecision.Start();
        }

        private string SelectAIRotateDecision(GenericShip ship)
        {
            var stressPriority = ship.GetAIStressPriority();

            if (!ActionsHolder.HasTarget(ship) || stressPriority >= 50)
            {
                var enemies = ship.SectorsInfo.GetEnemiesInAllSectors();
                var frontPriority = enemies[Arcs.ArcFacing.Front].Sum(s => s.PilotInfo.Cost);
                var leftPriority = enemies[Arcs.ArcFacing.Left].Sum(s => s.PilotInfo.Cost) + stressPriority;
                var rightPriority = enemies[Arcs.ArcFacing.Right].Sum(s => s.PilotInfo.Cost) + stressPriority;

                if (leftPriority > 0 && leftPriority > rightPriority && leftPriority > frontPriority)
                    return "Left";
                if (rightPriority > 0 && rightPriority > frontPriority)
                    return "Right";
            }

            return "Skip";
        }

        private void RotateTractoredShip(Direction direction, Action callback)
        {
            //We need to change Selection.ThisShip before rotating. Making sure that we always change back afterwards
            var selectedShip = Selection.ThisShip;
            Selection.ThisShip = TheShip;

            Action resetSelection = () => 
            {
                Selection.ThisShip = selectedShip;
                callback();
            };

            Action assignStress = () =>
            {
                TheShip.Tokens.AssignToken(typeof(StressToken), resetSelection);
            };
            
            if (direction == Direction.Left) TheShip.Rotate90Counterclockwise(assignStress);
            else if (direction == Direction.Right) TheShip.Rotate90Clockwise(assignStress);
            else resetSelection();
        }

        private void CheckObstacles()
        {
            Rules.AsteroidHit.CheckHits(TheShip);
            Rules.AsteroidLanded.CheckLandedOnObstacle(TheShip);
            Triggers.ResolveTriggers(TriggerTypes.OnMovementFinish, Next);
        }

        public override void Next()
        {
            Phases.CurrentSubPhase = PreviousSubPhase;
            UpdateHelpInfo();

            CallBack();
        }

        public override void Resume()
        {
            var prevPhase = Phases.CurrentSubPhase;
            Phases.CurrentSubPhase = this;
            UpdateHelpInfo();
            // TODO: Check barrel roll problems
            /*if ((prevPhase is BarrelRollPlanningSubPhase) && (prevPhase as BarrelRollPlanningSubPhase).CheckBarrelRollProblems().Count > 0) {
                RegisterTractorPlanning();
            }*/
        }

        protected class TractorBeamDirectionDecisionSubPhase : DecisionSubPhase { }
    }
}
