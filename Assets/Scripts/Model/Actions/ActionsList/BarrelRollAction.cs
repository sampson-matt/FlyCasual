using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.Aggressor;
using BoardTools;
using SubPhases;
namespace ActionsList
{

    public class BarrelRollAction : GenericAction
    {
        public bool IsThroughObstacle { get; set; }

        public ManeuverTemplate SelectedTemplate { get; set; }

        public BarrelRollAction()
        {
            Name = "Barrel Roll";
        }

        public override void ActionTake()
        {
            if (Selection.ThisShip.Owner.UsesHotacAiRules)
            {
                Phases.CurrentSubPhase.CallBack();
            }
            else
            {
                Phases.CurrentSubPhase.Pause();

                BarrelRollPlanningSubPhase subphase = Phases.StartTemporarySubPhaseNew<BarrelRollPlanningSubPhase>(
                    "Barrel Roll",
                    Phases.CurrentSubPhase.CallBack
                );
                subphase.HostAction = this;
                subphase.Start();
            }
        }

        public override void RevertActionOnFail(bool hasSecondChance = false)
        {
            Phases.GoBack();
        }

        // Barrel rolls that improve the AI ship's position, best first; filled when the AI considers this action
        public List<RepositionOption> AiRepositionOptions { get; set; }

        public override int GetActionPriority()
        {
            AiRepositionOptions = null;
            if (!RepositionSubSystem.IsEnabledFor(Selection.ThisShip)) return 0;

            int result = RepositionSubSystem.GetBarrelRollPriority(Selection.ThisShip, this, out List<RepositionOption> options);
            AiRepositionOptions = options;

            return result;
        }

    }

}
