using OutwardChatCommandsManager.Commands;
using OutwardChatCommandsManager.Utility.Enums;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Events
{
    public static class EventBusPublisher
    {
        public const string Event_AddedCommand = "ChatCommandsManager@AddChatCommand_After";
        public const string Event_RemovedCommand = "ChatCommandsManager@RemoveChatCommand_After";

        public static void SendAddedCommand(ChatCommand chatCommand)
        {
            var payload = new EventPayload
            {
                [EventRegistryParamsHelper.Get(EventRegistryParams.CommandName).key] = chatCommand.Name,
                [EventRegistryParamsHelper.Get(EventRegistryParams.CommandDescription).key] = chatCommand.Description,
                [EventRegistryParamsHelper.Get(EventRegistryParams.CommandParameters).key] = chatCommand.Parameters,
                [EventRegistryParamsHelper.Get(EventRegistryParams.CommandAction).key] = chatCommand.Function,
                [EventRegistryParamsHelper.Get(EventRegistryParams.CommandRequiresDebugMode).key] = chatCommand.RequireDebugMode,
            };
            EventBus.Publish(OCCM.GUID, Event_AddedCommand, payload);
        }

        public static void SendRemovedCommand(ChatCommand chatCommand)
        {
            var payload = new EventPayload
            {
                [EventRegistryParamsHelper.Get(EventRegistryParams.CommandName).key] = chatCommand.Name,
                [EventRegistryParamsHelper.Get(EventRegistryParams.CommandDescription).key] = chatCommand.Description,
                [EventRegistryParamsHelper.Get(EventRegistryParams.CommandParameters).key] = chatCommand.Parameters,
                [EventRegistryParamsHelper.Get(EventRegistryParams.CommandAction).key] = chatCommand.Function,
                [EventRegistryParamsHelper.Get(EventRegistryParams.CommandRequiresDebugMode).key] = chatCommand.RequireDebugMode,
            };
            EventBus.Publish(OCCM.GUID, Event_RemovedCommand, payload);
        }
    }
}
