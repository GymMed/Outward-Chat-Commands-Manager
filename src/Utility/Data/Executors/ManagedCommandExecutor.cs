using OutwardChatCommandsManager.Utility.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Data.Executors
{
    public sealed class ManagedCommandExecutor : ICommandExecutor
    {
        public void Execute(ChatCommandInvocation invocation)
        {
            invocation.Command.TriggerFunction(invocation.Character, invocation.Arguments);
        }
    }
}
