using OutwardChatCommandsManager.Commands;
using OutwardChatCommandsManager.Managers;
using OutwardChatCommandsManager.Utility.Data.Interfaces;
using OutwardChatCommandsManager.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Data
{
    public class ChatCommandInvocation
    {
        private Character _character;
        private IChatCommand _command;
        private Dictionary<string, string> _arguments;
        private string _message;
        private ICommandExecutor executor;

        public ChatCommandInvocation(Character character, IChatCommand command, Dictionary<string, string> arguments, ICommandExecutor executor, string message = null)
        {
            Character = character;
            Command = command;
            Arguments = arguments;
            Message = message;
            Executor = executor;
        }

        public ChatCommandInvocation(Character character, IChatCommand command, ICommandExecutor executor, string message = null)
        {
            Character = character;
            Command = command;
            Message = message;
            Arguments = ChatHelpers.GetArgumentsForCommand(message, command);
            Executor = executor;
        }

        public Character Character { get => _character; set => _character = value; }
        public IChatCommand Command { get => _command; set => _command = value; }
        public Dictionary<string, string> Arguments { get => _arguments; set => _arguments = value; }
        public string Message { get => _message ; set => _message = value; }
        public ICommandExecutor Executor { get => executor; set => executor = value; }

        public void TriggerFunction()
        {
            if(Command == null)
            {
                OCCM.LogMessage("ChatCommandInvocation@TriggerFunction doesn't have a command to trigger!");
                return;
            }

            if(Character == null)
            {
                OCCM.LogMessage("ChatCommandInvocation@TriggerFunction doesn't have a character triggering command!");
                return;
            }

            if(Arguments == null)
            {
                OCCM.LogMessage("ChatCommandInvocation@TriggerFunction doesn't have provided arguments to trigger command!");
                return;
            }

            ChatCommandsManager.History.Add(this);
            Executor.Execute(this);
        }
    }
}
