using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubPhases;

namespace MainPhases
{

    public class SetupPhase : GenericPhase
    {

        public override void StartPhase()
        {
            Name = "Setup Phase";

            StartGameStartSubPhase();
        }

        private void StartGameStartSubPhase()
        {
            Phases.CurrentSubPhase = new GameStartSubPhase();
            Phases.CurrentSubPhase.Start();
            Phases.CurrentSubPhase.Prepare();
            Phases.CurrentSubPhase.Initialize();
        }

        public override void NextPhase()
        {
            Selection.DeselectAllShips();
            Phases.CurrentPhase = new PlanningPhase();
            Phases.CurrentPhase.StartPhase();
        }

    }

}