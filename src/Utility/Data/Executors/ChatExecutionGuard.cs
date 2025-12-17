using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Data.Executors
{
    public static class ChatExecutionGuard
    {
        [ThreadStatic]
        public static bool IsForwarding;
    }
}
