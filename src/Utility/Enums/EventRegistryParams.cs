using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Enums
{
    public enum EventRegistryParams
    {
        CommandName,
        CommandParameters,
        CommandAction,
        IsCheatCommand,
        CommandDescription,
        CommandRequiresDebugMode,
    }

    public static class EventRegistryParamsHelper
    {
        private static readonly Dictionary<EventRegistryParams, (string key, Type type, string description)> _registry
            = new()
            {
                [EventRegistryParams.CommandName] = ("command", typeof(string), "Required. The text players type in chat (e.g. \"/command\") to trigger your handler."),
                [EventRegistryParams.CommandParameters] = ("parameters", typeof(Dictionary<string, (string, string)>), "Optional. Parameters with name and (description, default value) provided as dictionary."),
                [EventRegistryParams.CommandAction] = ("function", typeof(Action<Character, Dictionary<string, string>>), "Required. The method to execute when the command is triggered. Character is the command caller and Dictionary stores parameter and argument(value)."),
                [EventRegistryParams.IsCheatCommand] = ("isCheatCommand", typeof(bool), "Optional. Default false. Determines if player game will be saved after triggering your command."),
                [EventRegistryParams.CommandDescription] = ("description", typeof(string), "Optional. Default null. Provides description of command for users."),
                [EventRegistryParams.CommandRequiresDebugMode] = ("debugMode", typeof(bool), "Optional. Default false. Determines if command requires debug mode to work."),
            };

        public static (string key, Type type, string description) Get(EventRegistryParams param) => _registry[param];

        public static (string key, Type type, string description)[] Combine(
            params object[] items)
        {
            var list = new List<(string key, Type type, string description)>();

            foreach (var item in items)
            {
                if (item is ValueTuple<string, Type, string> single)
                {
                    list.Add(single);
                }
                else if (item is ValueTuple<string, Type, string>[] array)
                {
                    list.AddRange(array);
                }
                else
                {
                    throw new ArgumentException(
                        $"Unsupported item type: {item?.GetType().FullName}");
                }
            }

            return list.ToArray();
        }
    }
}
