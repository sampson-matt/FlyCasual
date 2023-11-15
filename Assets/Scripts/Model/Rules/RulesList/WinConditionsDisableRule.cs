using ExtraOptions.ExtraOptionsList;
using Players;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using Ship;

namespace RulesList
{
    public class WinConditionsDisableRule : WinConditionsRule
    {
        private GenericShip disableShip;
        private Direction escapeDirection;
        private string ship;
        private string victoryMessage;
        private string defeatMessage;

        public WinConditionsDisableRule(string ship, Direction escapeDirection, string victoryMessage, string defeatMessage)
        {
            this.escapeDirection = escapeDirection;
            this.ship = ship;
            this.victoryMessage = victoryMessage;
            this.defeatMessage = defeatMessage;

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            Phases.Events.OnSetupEnd += SetEscapeShip;
            Phases.Events.OnRoundEnd += CheckWinConditions;
        }

        private void SetEscapeShip()
        {
            disableShip = Roster.Player2.Ships.Values.Where(s => s.PilotNameCanonical.Equals(ship)).Last();

            disableShip.OnOffTheBoard += CheckEscape;
            disableShip.OnShipIsReadyToBeRemoved += ShipRemoved;
        }

        private void ShipRemoved(GenericShip ship)
        {
            Messages.ShowInfo(disableShip.PilotInfo.PilotName + " has been destroyed.");
            if (DebugManager.BatchAiSquadTestingModeActive)
            {
                BatchAiSquadsTestingModeExtraOption.Results[Tools.IntToPlayer(2)]++;
            }
            UI.AddTestLogEntry(GetPlayerName(2) + " Wins!");
            UI.ShowGameResults("Imperial Victory!");
            if (DebugManager.ReleaseVersion) AnalyticsEvent.GameOver();

            Rules.FinishGame();
        }

        private void CheckEscape(ref bool shouldBeDestroyed, Direction direction)
        {
            shouldBeDestroyed = false;

            Messages.ShowInfo(disableShip.PilotInfo.PilotName + " has escaped.");

            Roster.MoveToReserve(disableShip);

            UI.AddTestLogEntry(GetPlayerName(2) + " Wins!");
            UI.ShowGameResults("Imperial Victory!");
            if (DebugManager.ReleaseVersion) AnalyticsEvent.GameOver();
            
            Rules.FinishGame();
        }

        public override void CheckWinConditions()
        {
            int eliminatedTeam = Roster.CheckIsAnyTeamIsEliminated();

            if (disableShip.IsDestroyed)
            {
                eliminatedTeam = 1;
                Messages.ShowInfo(disableShip.PilotInfo.PilotName + " has been destroyed.");
            }

            if (eliminatedTeam != 0)
            {
                if (eliminatedTeam == 1)
                {
                    if (DebugManager.BatchAiSquadTestingModeActive)
                    {
                        BatchAiSquadsTestingModeExtraOption.Results[Tools.IntToPlayer(Roster.AnotherPlayer(eliminatedTeam))]++;
                    }
                    UI.AddTestLogEntry(GetPlayerName(Roster.AnotherPlayer(eliminatedTeam)) + " Wins!");
                    UI.ShowGameResults("Imperial Victory!");
                }
                else
                {
                    if (DebugManager.BatchAiSquadTestingModeActive)
                    {
                        BatchAiSquadsTestingModeExtraOption.Results[PlayerNo.Player1]++;
                        BatchAiSquadsTestingModeExtraOption.Results[PlayerNo.Player2]++;
                    }
                    UI.AddTestLogEntry("Draw!");
                    UI.ShowGameResults("Draw!");
                }

                if (DebugManager.ReleaseVersion) AnalyticsEvent.GameOver();

                Rules.FinishGame();
            }

            if (Roster.GetPlayer(PlayerNo.Player2).Ships.Count() == 1 && disableShip.State.IsDisabled)
            {
                UI.AddTestLogEntry(GetPlayerName(1) + " Wins!");
                UI.ShowGameResults("Rebel Victory!");
                if (DebugManager.ReleaseVersion) AnalyticsEvent.GameOver();

                Rules.FinishGame();
            }
            
        }

        private string GetPlayerName(int playerNo)
        {
            if (Roster.GetPlayer(1).NickName != Roster.GetPlayer(2).NickName)
            {
                return Roster.GetPlayer(playerNo).NickName;
            }
            else
            {
                return "Player " + playerNo;
            }
        }
    }
}
