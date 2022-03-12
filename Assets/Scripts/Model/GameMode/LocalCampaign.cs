using UnityEngine;
using Players;
using GameCommands;
using SquadBuilderNS;

namespace GameModes
{
    public class LocalCampaign : GameMode
    {
        public override string Name { get { return "LocalCampaign"; } }

        public override void ExecuteCommand(GameCommand command)
        {
            GameController.SendCommand(command);
        }

        public override void ExecuteServerCommand(GameCommand command)
        {
            GameController.SendCommand(command);
        }

        public override void GiveInitiativeToPlayer(int playerNo)
        {
            if (ReplaysManager.Mode == ReplaysMode.Write)
            {
                GameController.SendCommand
                (
                    RulesList.InitiativeRule.GenerateInitiativeDecisionOwnerCommand(playerNo)
                );
            }
            else if (ReplaysManager.Mode == ReplaysMode.Read)
            {
                // GameController.WaitForCommand();
            }
        }

        public override void StartBattle()
        {
            CampaignLoader.LoadCampaign();
            Global.BattleIsReady();
        }

        public override void GenerateDamageDeck(PlayerNo playerNo, int seed)
        {
            SyncDamageDeckSeed(playerNo, seed);
        }

        private void SyncDamageDeckSeed(PlayerNo playerNo, int seed)
        {
            // TODO: Move to player types

            if (ReplaysManager.Mode == ReplaysMode.Write)
            {
                GameController.SendCommand
                (
                    DamageDecks.GenerateDeckShuffleCommand(playerNo, seed)
                );
            }
            else if (ReplaysManager.Mode == ReplaysMode.Read)
            {
                // GameController.WaitForCommand();
            }
        }

        public override void ReturnToMainMenu()
        {
            Global.ReturnToMainMenu();
        }

        public override void QuitToDesktop()
        {
            Application.Quit();
        }
    }
}
