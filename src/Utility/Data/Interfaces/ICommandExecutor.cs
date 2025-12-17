using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Data.Interfaces
{
    public interface ICommandExecutor
    {
        void Execute(ChatCommandInvocation invocation);
    }
}
