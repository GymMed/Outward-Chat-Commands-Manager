using OutwardChatCommandsManager.Commands;
using OutwardChatCommandsManager.Events;
using OutwardChatCommandsManager.Utility.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Managers
{
    public class ChatCommandsManager
    {
        private static ChatCommandsManager _instance;

        private ChatCommandsManager()
        {
        }

        public static ChatCommandsManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ChatCommandsManager();

                return _instance;
            }
        }

        private static Dictionary<string, ChatCommand> _chatCommands = new();
        private static bool _hasUsedCheat = false;
        private static ChatCommandHistory _history = new();

        public static Dictionary<string, ChatCommand> ChatCommands { get => _chatCommands; set => _chatCommands = value; }
        public static bool HasUsedCheat { get => _hasUsedCheat; set => _hasUsedCheat = value; }
        public static ChatCommandHistory History { get => _history; set => _history = value; }

        public void RestoreHistory(List<ChatCommandInvocation> oldHistory)
        {
            History = new(oldHistory);
        }

        public void AddChatCommand(ChatCommand chatCommand)
        {
            if(string.IsNullOrEmpty(chatCommand.Name))
            {
                OCCM.LogMessage("ChatCommandManager@AddChatCommand Tried to create empty chat command!");
                return;
            }

            if(ChatCommands.ContainsKey(chatCommand.Name))
            {
                OCCM.LogMessage($"ChatCommandManager@AddChatCommand Command already exist with name {chatCommand.Name}!");
                return;
            }

            ChatCommands.Add(chatCommand.Name, chatCommand);
            EventBusPublisher.SendAddedCommand(chatCommand);
        }

        public void RemoveChatCommand(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                OCCM.LogMessage("ChatCommandManager@RemoveChatCommand Tried to remove empty chat command!");
                return;
            }

            if(!ChatCommands.TryGetValue(name, out ChatCommand chatCommand))
            {
                return;
            }

            ChatCommands.Remove(name);
            EventBusPublisher.SendRemovedCommand(chatCommand);
        }
    }
}
