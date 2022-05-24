using ExtraOptions.ExtraOptionsList;
using Players;
using UnityEngine;
using UnityEngine.Analytics;

namespace RulesList
{
    public abstract class WinConditionsRule
    {

        //public WinConditionsRule()
        //{
        //    SubscribeEvents();
        //}

        //private void SubscribeEvents()
        //{
        //    Phases.Events.OnRoundEnd += CheckWinConditions;
        //}

        public abstract void CheckWinConditions();
    }
}
