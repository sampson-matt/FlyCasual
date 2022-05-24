using ExtraOptions.ExtraOptionsList;
using Players;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

namespace RulesList
{
    public class WinConditionsDestroyRule : WinConditionsRule
    {
        private int remainingEnemies;
        private string enemyType;
        private string victoryMessage;
        private string defeatMessage;

        public WinConditionsDestroyRule(string enemyType, int remainingEnemies, string victoryMessage, string defeatMessage)
        {
            this.enemyType = enemyType;
            this.remainingEnemies = remainingEnemies;
            this.victoryMessage = victoryMessage;
            this.defeatMessage = defeatMessage;

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            Phases.Events.OnRoundEnd += CheckWinConditions;
        }

        public override void CheckWinConditions()
        {
            int eliminatedTeam = Roster.CheckIsAnyTeamIsEliminated();

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
            if(enemyType.Equals("minefield"))
            {
                if (Roster.GetPlayer(PlayerNo.Player2).Remotes.Where(r => r.Value.GetType().Equals(typeof(Remote.MineField))).Count() < remainingEnemies)
                {
                    UI.AddTestLogEntry(GetPlayerName(1) + " Wins!");
                    UI.ShowGameResults("Rebel Victory!");
                    if (DebugManager.ReleaseVersion) AnalyticsEvent.GameOver();

                    Rules.FinishGame();
                }
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
