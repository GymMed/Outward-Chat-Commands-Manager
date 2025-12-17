using OutwardChatCommandsManager.Managers;
using OutwardChatCommandsManager.Utility.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Commands
{
    public class ChatCommand : IChatCommand
    {
        private string _name;
        private Dictionary<string, (string, string)> _parameters;
        private string _description;
        private bool _isCheat;
        private bool _requireDebugMode;
        private Action<Character, Dictionary<string, string>> _function;

        public string Name { get => _name; set => _name = value; }
        public Dictionary<string, (string, string)> Parameters { get => _parameters; set => _parameters = value; }
        public string Description { get => _description; set => _description = value; }
        public bool IsCheat { get => _isCheat; set => _isCheat = value; }
        public bool RequireDebugMode { get => _requireDebugMode; set => _requireDebugMode = value; }
        public Action<Character, Dictionary<string, string>> Function { get => _function; set => _function = value; }

        public ChatCommand(string name, Dictionary<string, (string, string)> parameters, string description, bool isCheat, Action<Character, Dictionary<string, string>> function, bool requireDebugMode = false)
        {
            Name = name;
            Parameters = parameters;
            Description = description;
            IsCheat = isCheat;
            Function = function;
            RequireDebugMode = requireDebugMode;
        }

        public virtual void TriggerFunction(Character character, Dictionary<string, string> arguments)
        {
            if (RequireDebugMode)
            {
                if (!Global.CheatsEnabled)
                    return;
            }

            if (IsCheat)
                ChatCommandsManager.HasUsedCheat = true;

            Function?.Invoke(character, arguments);
        }
    }
}
