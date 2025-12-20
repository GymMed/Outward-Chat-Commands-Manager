using OutwardChatCommandsManager.Utility.Data.Executors;
using OutwardChatCommandsManager.Utility.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Enums
{
    public enum CommandExecutors
    {
        OriginalCommand,
        ChatCommand
    }

    public static class CommandExecutorsHelper
    {
        public static Dictionary<CommandExecutors, ICommandExecutor> Executors(Character character)
        {
            if(character?.CharacterUI?.ChatPanel == null)
            {
                OCCM.LogMessage("CommandExecutorsHelper@Executors Chracter panel is missing!");
                return null;
            }

            return new Dictionary<CommandExecutors, ICommandExecutor>()
            {
                { CommandExecutors.OriginalCommand, new GameChatForwardExecutor(character.CharacterUI.ChatPanel) },
                { CommandExecutors.ChatCommand, new ManagedCommandExecutor() }
            };
        }

        public static ICommandExecutor GetCommandExecutor(CommandExecutors executor, Character character)
        {
            Dictionary<CommandExecutors, ICommandExecutor> executors = Executors(character);

            if(executors == null)
                return new ManagedCommandExecutor();

            return executors[executor];
        }
    }
}
