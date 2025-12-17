using OutwardChatCommandsManager.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Data.Interfaces
{
    public interface IChatCommand
    {
        public string Name { get; set; }
        public Dictionary<string, (string, string)> Parameters { get; set; }
        public string Description { get; set; }
        public bool IsCheat { get; set; }
        public bool RequireDebugMode { get; set; }

        public void TriggerFunction(Character character, Dictionary<string, string> Arguments);
    }
}
