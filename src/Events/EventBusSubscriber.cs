using OutwardChatCommandsManager.Commands;
using OutwardChatCommandsManager.Managers;
using OutwardChatCommandsManager.Utility.Enums;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Events
{
    public static class EventBusSubscriber
    {
        public const string Event_AddCommand = "ChatCommandsManager@AddChatCommand";
        public const string Event_RemoveCommand = "ChatCommandsManager@RemoveChatCommand";

        public static void AddSubscribers()
        {
            EventBus.Subscribe(OCCM.EVENTS_LISTENER_GUID, Event_AddCommand, AddCommand);
            EventBus.Subscribe(OCCM.EVENTS_LISTENER_GUID, Event_RemoveCommand, RemoveCommand);
        }

        public static void AddCommand(EventPayload payload)
        {
            if (payload == null) return;

            (string key, Type type, string description) nameParameter = EventRegistryParamsHelper.Get(EventRegistryParams.CommandName);
            string commandName = payload.Get<string>(nameParameter.key, null);

            if(string.IsNullOrEmpty(commandName))
            {
                OCCM.LogMessage($"command is required field! Name your command.");
                return;
            }

            (string key, Type type, string description) functionParameter = EventRegistryParamsHelper.Get(EventRegistryParams.CommandAction);
            Action<Character, Dictionary<string, string>> function = payload.Get<Action<Character, Dictionary<string, string>>>(functionParameter.key, null);

            if(function == null)
            {
                OCCM.LogMessage($"function is required field! Please define what your command will do.");
                return;
            }

            (string key, Type type, string description) commandParamsParameter = EventRegistryParamsHelper.Get(EventRegistryParams.CommandParameters);
            Dictionary<string, (string, string)> commandParams = payload.Get<Dictionary<string, (string, string)>>(commandParamsParameter.key, null);

            (string key, Type type, string description) descriptionParameter = EventRegistryParamsHelper.Get(EventRegistryParams.CommandDescription);
            string description = payload.Get<string>(descriptionParameter.key, null);

            (string key, Type type, string description) isCheatParameter = EventRegistryParamsHelper.Get(EventRegistryParams.IsCheatCommand);
            bool isCheatCommand = payload.Get<bool>(isCheatParameter.key, false);

            (string key, Type type, string description) debugParameter = EventRegistryParamsHelper.Get(EventRegistryParams.CommandRequiresDebugMode);
            bool requiresDebug = payload.Get<bool>(debugParameter.key, false);

            ChatCommandsManager.Instance.AddChatCommand(new ChatCommand(commandName, commandParams, description, isCheatCommand, function, requiresDebug));
        }

        public static void RemoveCommand(EventPayload payload)
        {
            if (payload == null) return;

            (string key, Type type, string description) nameParameter = EventRegistryParamsHelper.Get(EventRegistryParams.CommandName);
            string commandName = payload.Get<string>(nameParameter.key, null);

            if(string.IsNullOrEmpty(commandName))
            {
                OCCM.LogMessage($"command is required field! Name your command.");
                return;
            }

            ChatCommandsManager.Instance.RemoveChatCommand(commandName);
        }
    }
}
