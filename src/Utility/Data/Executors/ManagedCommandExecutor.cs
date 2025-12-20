using OutwardChatCommandsManager.Utility.Data.Interfaces;
using OutwardChatCommandsManager.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Data.Executors
{
    public sealed class ManagedCommandExecutor : ICommandExecutor
    {
        public CommandExecutors ExecutorType => CommandExecutors.ChatCommand;

        public void Execute(ChatCommandInvocation invocation)
        {
            invocation.Command.TriggerFunction(invocation.Character, invocation.Arguments);
        }
    }
}
