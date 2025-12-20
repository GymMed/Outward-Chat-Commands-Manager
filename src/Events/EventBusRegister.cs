using OutwardChatCommandsManager.Utility.Enums;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Events
{
    public static class EventBusRegister
    {
        private static readonly (string key, Type type, string description)[] CommandData =
        {
            EventRegistryParamsHelper.Get(EventRegistryParams.CommandName),
            EventRegistryParamsHelper.Get(EventRegistryParams.CommandDescription),
            EventRegistryParamsHelper.Get(EventRegistryParams.CommandParameters),
            EventRegistryParamsHelper.Get(EventRegistryParams.CommandAction),
            EventRegistryParamsHelper.Get(EventRegistryParams.IsCheatCommand),
            EventRegistryParamsHelper.Get(EventRegistryParams.CommandRequiresDebugMode),
        };

        public static void RegisterEvents()
        {
            EventBus.RegisterEvent(
                OCCM.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_AddCommand,
                "Adds command to chat commands manager.",
                CommandData
            );

            EventBus.RegisterEvent(
                OCCM.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_RemoveCommand,
                "Removes command from chat commands manager.",
                EventRegistryParamsHelper.Get(EventRegistryParams.CommandName)
            );

            EventBus.RegisterEvent(
                OCCM.GUID,
                EventBusPublisher.Event_AddedCommand,
                "Fires event when comment was added to chat commands manager.",
                CommandData
            );

            EventBus.RegisterEvent(
                OCCM.GUID,
                EventBusPublisher.Event_RemovedCommand,
                "Fires event when comment was removed from chat commands manager.",
                CommandData
            );
        }
    }
}
