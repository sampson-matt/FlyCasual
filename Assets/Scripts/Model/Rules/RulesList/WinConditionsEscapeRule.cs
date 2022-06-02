using ExtraOptions.ExtraOptionsList;
using Players;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using Ship;
using System.Collections.Generic;

namespace RulesList
{
    public class WinConditionsEscapeRule : WinConditionsRule
    {
        private GenericShip escapeShip;
        private Direction escapeDirection;
        private string ship;
        private string direction;
        private string victoryMessage;
        private string defeatMessage;

        public WinConditionsEscapeRule(string ship, Direction escapeDirection, string victoryMessage, string defeatMessage)
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
            escapeShip = Roster.Player1.Ships.Values.Where(s => s.PilotNameCanonical.Equals(ship)).Last();

            escapeShip.OnOffTheBoard += CheckEscape;
        }

        private void CheckEscape(ref bool shouldBeDestroyed, Direction direction)
        {
            if (direction == escapeDirection)
            {
                shouldBeDestroyed = false;

                Messages.ShowInfo(escapeShip.PilotInfo.PilotName + " has safely retreated.");

                Roster.MoveToReserve(escapeShip);

                UI.AddTestLogEntry(GetPlayerName(1) + " Wins!");
                UI.ShowGameResults("Rebel Victory!");
                if (DebugManager.ReleaseVersion) AnalyticsEvent.GameOver();
            }
            else
            {
                if (DebugManager.BatchAiSquadTestingModeActive)
                {
                    BatchAiSquadsTestingModeExtraOption.Results[Tools.IntToPlayer(2)]++;
                }
                if (DebugManager.ReleaseVersion) AnalyticsEvent.GameOver();
                UI.AddTestLogEntry(GetPlayerName(2) + " Wins!");
                UI.ShowGameResults("Imperial Victory!");
            }
            Rules.FinishGame();
        }

        public override void CheckWinConditions()
        {
            int eliminatedTeam = Roster.CheckIsAnyTeamIsEliminated();

            if(escapeShip.IsDestroyed)
            {
                eliminatedTeam = 1;
                Messages.ShowInfo(escapeShip.PilotInfo.PilotName + " has been destroyed.");
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
