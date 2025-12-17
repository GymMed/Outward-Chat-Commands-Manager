using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Data.Executors
{
    public sealed class GameChatForwardExecutor : Interfaces.ICommandExecutor
    {
        private readonly ChatPanel _panel;

        public GameChatForwardExecutor(ChatPanel panel)
        {
            _panel = panel;
        }

        public void Execute(ChatCommandInvocation invocation)
        {
            // SAFETY FLAG to prevent infinite loop
            ChatExecutionGuard.IsForwarding = true;

            try
            {
                _panel.m_chatEntry.text = invocation.Message;
                _panel.SendChatMessage(); // let the game handle it
            }
            finally
            {
                ChatExecutionGuard.IsForwarding = false;
            }
        }
    }
}
