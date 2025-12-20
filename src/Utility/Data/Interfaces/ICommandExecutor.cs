using OutwardChatCommandsManager.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Data.Interfaces
{
    public interface ICommandExecutor
    {
        CommandExecutors ExecutorType { get; }
        void Execute(ChatCommandInvocation invocation);
    }
}
