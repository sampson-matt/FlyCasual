using Actions;
using ActionsList;
using Ship;
using SubPhases;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class SquadLeader : GenericUpgrade, IVariableCost
    {
        public SquadLeader() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Squad Leader",
                UpgradeType.Talent,
                cost: 4,
                isLimited: true,
                addAction: new ActionInfo(typeof(CoordinateAction), ActionColor.Red),
                abilityType: typeof(Abilities.SecondEdition.SquadLeaderAbility)
            );
        }

        public void UpdateCost(GenericShip ship)
        {
            Dictionary<int, int> initiativeToCost = new Dictionary<int, int>()
            {
                {0, 2},
                {1, 4},
                {2, 5},
                {3, 7},
                {4, 9},
                {5, 10},
                {6, 12}
            };

            UpgradeInfo.Cost = initiativeToCost[ship.PilotInfo.Initiative];
        }
    }
}

namespace Abilities.SecondEdition
{
    //While you coordinate, the ship you choose can perform an action only if that action is also on your action bar.
    public class SquadLeaderAbility : GenericAbility
    {
        public override void ActivateAbility() { }

        public override void DeactivateAbility() { }

        public override void ActivateAbilityForSquadBuilder()
        {
            HostShip.ActionBar.AddGrantedAction(new SquadLeaderCoordinateAction() { }, HostUpgrade);
        }
        public override void DeactivateAbilityForSquadBuilder()
        {
            HostShip.ActionBar.RemoveGrantedAction(typeof(SquadLeaderCoordinateAction), HostUpgrade);
        }

        public class SquadLeaderCoordinateAction : CoordinateAction
        {
            public SquadLeaderCoordinateAction() : base()
            {
                Color = ActionColor.Red;
            }

            public override void ActionTake()
            {
                base.CoordinateActionData = HostShip.CallCheckCoordinateModeModification();

                if (CoordinateActionData.MaxTargets == 1)
                {
                    SquadLeaderTargetSubPhase subphase = Phases.StartTemporarySubPhaseNew<SquadLeaderTargetSubPhase>(
                        "Select target for Coordinate",
                        Phases.CurrentSubPhase.CallBack
                    );
                    subphase.HostAction = this;
                    subphase.Start();
                }
                else
                {
                    CoordinateMultiTargetSubPhase subphase = Phases.StartTemporarySubPhaseNew<CoordinateMultiTargetSubPhase>(

                        "Select targets for Coordinate",
                        Phases.CurrentSubPhase.CallBack
                    );
                    subphase.HostAction = this;

                    subphase.RequiredPlayer = HostShip.Owner.PlayerNo;

                    subphase.Filter = base.FilterCoordinateTargets;
                    subphase.MaxToSelect = CoordinateActionData.MaxTargets;
                    subphase.WhenDone = CoordinateTargets;
                    subphase.CoordinateActionData = CoordinateActionData;
                    subphase.GetAiPriority += CoordinateActionData.GetAiPriority;

                    subphase.DescriptionShort = "Coordinate Action";
                    subphase.DescriptionLong = "Select one or more other ships.\nThey will each perform an action.";

                    subphase.Start();
                }
            }
        }

        public class SquadLeaderTargetSubPhase : CoordinateTargetSubPhase
        {
            protected override List<GenericAction> GetPossibleActions()
            {
                List<GenericAction> targetActions = Selection.ThisShip.GetAvailableActions();
                return targetActions.Where(a => HostAction.HostShip.ActionBar.HasAction(a.GetType())).ToList();
            }
            protected override void PerformCoordinateEffect()
            {
                GenericShip coordinatingShip = Selection.ThisShip;
                Selection.ThisShip = base.TargetShip;
                GenericAction currentAction = ActionsHolder.CurrentAction;

                Triggers.RegisterTrigger(
                    new Trigger()
                    {
                        Name = "Coordinate",
                        TriggerOwner = Selection.ThisShip.Owner.PlayerNo,
                        TriggerType = TriggerTypes.OnFreeActionPlanned,
                        EventHandler = PerformFreeAction
                    }
                );
                MovementTemplates.ReturnRangeRuler();

                Triggers.ResolveTriggers(TriggerTypes.OnFreeActionPlanned, (System.Action)delegate {
                    Selection.ThisShip = coordinatingShip;
                    ActionsHolder.CurrentAction = currentAction;
                    Phases.FinishSubPhase(typeof(SquadLeaderTargetSubPhase));
                    CallBack();
                });
            }
        }
    }
}