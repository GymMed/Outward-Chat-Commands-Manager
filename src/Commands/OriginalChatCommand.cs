using Mono.Cecil;
using OutwardChatCommandsManager.Utility.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Commands
{
    public class OriginalChatCommand : IChatCommand
    {
        private string _name;
        private Dictionary<string, (string, string)> _parameters;
        private string _description;
        private bool _isCheat;
        private bool _requireDebugMode;

        public OriginalChatCommand(string name, Dictionary<string, (string, string)> parameters, string description, bool isCheat, bool requireDebugMode)
        {
            Name = name;
            Parameters = parameters;
            Description = description;
            IsCheat = isCheat;
            RequireDebugMode = requireDebugMode;
        }

        public string Name { get => _name; set => _name = value; }
        public Dictionary<string, (string, string)> Parameters { get => _parameters; set => _parameters = value; }
        public string Description { get => _description; set => _description = value; }
        public bool IsCheat { get => _isCheat; set => _isCheat = value; }
        public bool RequireDebugMode { get => _requireDebugMode; set => _requireDebugMode = value; }

        public void TriggerFunction(Character character, Dictionary<string, string> Arguments)
        {
        }
    }
}
