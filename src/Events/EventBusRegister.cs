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
        private static readonly (string key, Type type, string description)[] ExampleGroupParams =
        {
            EventRegistryParamsHelper.Get(EventRegistryParams.CommandName),
            EventRegistryParamsHelper.Get(EventRegistryParams.CommandDescription),
        };

        public static void RegisterEvents()
        {
            EventBus.RegisterEvent(
                OCCM.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_AddCommand,
                "Adds command to chat commands manager.",
                EventRegistryParamsHelper.Get(EventRegistryParams.CommandName),
                EventRegistryParamsHelper.Get(EventRegistryParams.CommandDescription),
                EventRegistryParamsHelper.Get(EventRegistryParams.IsCheatCommand),
                EventRegistryParamsHelper.Get(EventRegistryParams.CommandParameters),
                EventRegistryParamsHelper.Get(EventRegistryParams.CommandAction)
            );

            EventBus.RegisterEvent(
                OCCM.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_RemoveCommand,
                "Removes command from chat commands manager.",
                EventRegistryParamsHelper.Get(EventRegistryParams.CommandName)
            );

            /*
            // Example of grouping groups and single parameters for reusable cases
            EventBus.RegisterEvent(
                OutwardModPackTemplate.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_ExecuteMyCode,
                "My code/method description",
                EventRegistryParamsHelper.Combine(
                    EventRegistryParamsHelper.Get(EventRegistryParams.CallerGUID),
                    EventRegistryParamsHelper.Get(EventRegistryParams.TryEnchantMenu),
                    ExampleGroupParams
                )
            );
            */
        }
    }
}
