using OutwardChatCommandsManager.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Commands
{
    public class MessageCommand
    {
        private string _command = "";
        private List<string> _arguments;

        public MessageCommand(string message)
        {
            List<string> tokens = ChatHelpers.Tokenize(message);
            string commandName = "";

            if (tokens.Count > 0)
                commandName = tokens[0];

            List<string> arguments = tokens.Skip(1).ToList();

            Command = commandName;
            Arguments = arguments;
        }

        public string Command { get => _command; set => _command = value; }
        public List<string> Arguments { get => _arguments; set => _arguments = value; }
    }
}
