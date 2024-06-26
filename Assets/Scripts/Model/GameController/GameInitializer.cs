﻿using GameCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GameInitializer
{
    public static Type AcceptsCommandType { get; private set; }
    public static int CommandsReceived { get; set; }

    public static void SetState(Type state)
    {
        AcceptsCommandType = state;
        CommandsReceived = 0;

        GameController.WaitForCommand();
    }

    public static void TryExecute(GameCommand command)
    {
        if (command.GetType() == AcceptsCommandType)
        {
            GameController.ConfirmCommand();
            command.Execute();
            CommandsReceived++;

            if (command.GetType() == typeof(SyncPlayerWithInitiativeCommand)) AcceptsCommandType = null;
        }
    }
}
