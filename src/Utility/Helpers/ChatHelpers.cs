using OutwardChatCommandsManager.Commands;
using OutwardChatCommandsManager.Managers;
using OutwardChatCommandsManager.Utility.Data;
using OutwardChatCommandsManager.Utility.Data.Interfaces;
using OutwardChatCommandsManager.Utility.Enums;
using Photon.SocketServer.Numeric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Helpers
{
    public static class ChatHelpers
    {
        public static void SendChatLog(ChatPanel panel, string message, ChatLogStatus status = ChatLogStatus.Info)
        {
            panel.ChatMessageReceived("System", ChatLogStatusHelper.GetChatLogText(message, status));
        }

        public static void LogAllCommandsToChat(Character character)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;
            SendChatLog(panel, "Providing a list of all registered commands by Chat Commands Manager Mod:", ChatLogStatus.Warning);
            ChatLogAllChatCommands(character);
        }

        public static void ChatLogAllChatCommands(Character character)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            foreach(KeyValuePair<OriginalChatCommands, OriginalChatCommand> command in OriginalChatCommandsHelper.Commands)
            {
                SendChatLog(panel, command.Value.Name, ChatLogStatus.Success);
            }

            foreach(KeyValuePair<string, ChatCommand> command in ChatCommandsManager.ChatCommands)
            {
                SendChatLog(panel, command.Value.Name, ChatLogStatus.Success);
            }

            SendChatLog(panel, $"Total {OriginalChatCommandsHelper.Commands.Count() + ChatCommandsManager.ChatCommands.Count()} commands. {OriginalChatCommandsHelper.Commands.Count()} original commands. {ChatCommandsManager.ChatCommands.Count()} added commands.");
            SendChatLog(panel, "For more info use:\"/help\"");
        }

        public static void ShowCommandsHistory(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if (panel == null)
            {
                OCCM.LogMessage("ChatHelpers@ShowCommandsHistory Tried to log chat command to missing chatPanel.");
                return;
            }

            SendChatLog(panel, "History of chat commands invocations:");

            arguments.TryGetValue("detailed", out string isDetailed);

            if (!string.IsNullOrWhiteSpace(isDetailed))
            {
                foreach(ChatCommandInvocation invocation in ChatCommandsManager.History.Items)
                {
                    // separator
                    SendChatLog(panel, "");
                    ChatLogChatCommand(panel, invocation.Command);
                    ChatLogChatCommandArguments(panel, invocation.Arguments);
                }
                return;
            }

            string commandName = null;

            foreach (ChatCommandInvocation invocation in ChatCommandsManager.History.Items)
            {
                commandName = invocation.Command?.Name;

                if (string.IsNullOrEmpty(commandName))
                    commandName = "null";

                SendChatLog(panel, commandName, ChatLogStatus.Success);
            }
        }

        public static void LockKey(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OCCM.LogMessage("ChatHelpers@ChatLogChatCommand Tried to log chat command to missing chatPanel.");
                return;
            }

            arguments.TryGetValue("index", out string lockKeyIndex);

            if (string.IsNullOrWhiteSpace(lockKeyIndex))
            {
                lockKeyIndex = "1";
            }

            if(!int.TryParse(lockKeyIndex, out int keyIndex))
            {
                int firstSlot = (int)HotkeySlots.Slot1 + 1;
                int lastSlot = Enum.GetValues(typeof(HotkeySlots)).Length;
                SendChatLog(panel, $"Error: \"Provided incorrect {lockKeyIndex} lock key index. Can use from {firstSlot} to {lastSlot}. " +
                    $"Ex.: \"/lockKey 1\"\"", ChatLogStatus.Error);
                return;
            }

            if (!arguments.TryGetValue("show", out string showArg) || string.IsNullOrWhiteSpace(showArg))
            {
                var history = ChatCommandsManager.History.Items;
                int totalHistorySize = history.Count();
                
                if(totalHistorySize < 2)
                {
                    SendChatLog(panel, $"No previous command to lock to key.", ChatLogStatus.Warning);
                    return;
                }
                ChatCommandInvocation invocation = history[totalHistorySize - 2];
                HotkeySlotsHelper.Lock((HotkeySlots)(keyIndex - 1), invocation);
                SendChatLog(panel, $"Successfully assigned lock key {keyIndex} to {invocation.Command?.Name}.", ChatLogStatus.Success);
                return;
            }

            if(!string.Equals(showArg, "show", StringComparison.OrdinalIgnoreCase))
            {
                SendChatLog(panel, $"Provided argument \"{showArg}\" is incorrect! It can only be \"show\" or left empty.", ChatLogStatus.Error);
                return;
            }

            if(!HotkeySlotsHelper.TryGet((HotkeySlots)(keyIndex - 1), out ChatCommandInvocation assignedInvocation))
            {
                SendChatLog(panel, $"Your slot key {lockKeyIndex} doesn't have assigned chat command.", ChatLogStatus.Warning);
                return;
            }

            SendChatLog(panel, $"Lock key {keyIndex} was assigned to {assignedInvocation.Command.Name} chat command with arguments:");
            ChatLogChatCommandArguments(panel, assignedInvocation.Arguments);
        }

        public static void ReleaseKey(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OCCM.LogMessage("ChatHelpers@ChatLogChatCommand Tried to log chat command to missing chatPanel.");
                return;
            }
            arguments.TryGetValue("index", out string lockKeyIndex);

            if (string.IsNullOrWhiteSpace(lockKeyIndex))
            {
                lockKeyIndex = "1";
            }

            if(!int.TryParse(lockKeyIndex, out int keyIndex))
            {
                int firstSlot = (int)HotkeySlots.Slot1 + 1;
                int lastSlot = Enum.GetValues(typeof(HotkeySlots)).Length;
                SendChatLog(panel, $"Error: \"Provided incorrect {keyIndex} lock key index. Can use from {firstSlot} to {lastSlot}. " +
                    $"Ex.: \"/lockKey 1\"\"", ChatLogStatus.Error);
                return;
            }

            HotkeySlots currentSlot = (HotkeySlots)(keyIndex - 1);
            if(!HotkeySlotsHelper.TryGet(currentSlot, out ChatCommandInvocation assignedInvocation))
            {
                SendChatLog(panel, $"Your slot key {lockKeyIndex} doesn't have assigned chat command.", ChatLogStatus.Warning);
                return;
            }

            HotkeySlotsHelper.Release(currentSlot);

            SendChatLog(panel, $"Lock key {keyIndex} was released from {assignedInvocation.Command.Name} chat command with arguments:", ChatLogStatus.Success);
            ChatLogChatCommandArguments(panel, assignedInvocation.Arguments);
        }

        public static void ChatLogChatCommand(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OCCM.LogMessage("ChatHelpers@ChatLogChatCommand Tried to log chat command to missing chatPanel.");
                return;
            }
            
            arguments.TryGetValue("command", out string command);

            if(string.IsNullOrWhiteSpace(command))
            {
                if(!ChatCommandsManager.ChatCommands.TryGetValue("help", out ChatCommand helpCommand))
                {
                    SendChatLog(panel, "help command not found!", ChatLogStatus.Error);
                    return;
                }

                SendChatLog(panel, "Help command triggered!");
                ChatLogChatCommand(panel, helpCommand);
                return;
            }

            SendChatLog(panel, "Help command triggered!");
            if(!ChatCommandsManager.ChatCommands.TryGetValue(command, out ChatCommand argCommand))
            {
                if (!OriginalChatCommandsHelper.CommandsByName.TryGetValue(command, out OriginalChatCommand originalArgCommand))
                {
                    SendChatLog(panel, $"{command} command not found! Are you sure it exist? Type:\"/commands\"", ChatLogStatus.Error);
                    return;
                }

                ChatLogChatCommand(panel, originalArgCommand);
                return;
            }

            ChatLogChatCommand(panel, argCommand);
        }

        public static void ChatLogChatCommand(ChatPanel panel, IChatCommand chatCommand)
        {
            if(panel == null)
            {
                OCCM.LogMessage("ChatHelpers@ChatLogChatCommand Tried to log chat command to missing chatPanel.");
                return;
            }

            SendChatLog(panel, $"Command Name: {chatCommand.Name}", ChatLogStatus.Success);
            SendChatLog(panel, $"Command Description: {chatCommand.Description}", ChatLogStatus.Success);
            SendChatLog(panel, $"Command Parameters: ", ChatLogStatus.Success);
            ChatLogChatCommandParameters(panel, chatCommand);
            SendChatLog(panel, $"Command Is Cheat: {chatCommand.IsCheat}", ChatLogStatus.Success);
            SendChatLog(panel, $"Command Require Debug Mode: {chatCommand.RequireDebugMode}", ChatLogStatus.Success);
        }

        public static void ChatLogChatCommandParameters(ChatPanel panel, IChatCommand chatCommand)
        {
            foreach(KeyValuePair<string, (string, string)> parameter in chatCommand.Parameters)
            {
                string value = string.IsNullOrEmpty(parameter.Value.Item2) ? "null" : parameter.Value.Item2;
                SendChatLog(panel, $"-parameter name: {parameter.Key} => default value: {value}\n" +
                    $"-description: {parameter.Value.Item1}", ChatLogStatus.Success);
            }
        }

        public static void ChatLogChatCommandArguments(ChatPanel panel, Dictionary<string, string> arguments)
        {
            string finalMessage = "";

            foreach(KeyValuePair<string, string> argument in arguments)
            {
                if (string.IsNullOrWhiteSpace(argument.Value))
                    finalMessage = $"-parameter name: {argument.Key}";
                else
                    finalMessage = $"-parameter name: {argument.Key} => argument value: {argument.Value}";

                SendChatLog(panel, finalMessage, ChatLogStatus.Success);
            }
        }

        public static ChatPanel GetChatPanel(Character character)
        {
            return character?.CharacterUI?.ChatPanel;
        }

        public static bool TryGetValidCommand(string message, out IChatCommand command)
        {
            command = null;

            if (message.StartsWith("/"))
            {
                MessageCommand messageCommand = new MessageCommand(message);
                string commandName = messageCommand.Command.Substring(1);
                IChatCommand matchCommand = null;

                if (!ChatCommandsManager.ChatCommands.TryGetValue(commandName, out ChatCommand createdCommand))
                {
                    if (!OriginalChatCommandsHelper.CommandsByName.TryGetValue(commandName, out OriginalChatCommand originalCommand))
                    {
                        return false;
                    }
                    matchCommand = originalCommand;
                }
                else
                    matchCommand = createdCommand;

                if(matchCommand.RequireDebugMode && !Global.CheatsEnabled)
                    return false;

                if(messageCommand.Arguments.Count > matchCommand.Parameters.Count)
                {
                    OCCM.LogMessage($"Command {matchCommand.Name} retrieved too many arguments from user.");
                    return false;
                }

                command = matchCommand;
                return true;
            }

            return false;
        }

        public static bool HasValidCommand(string message)
        {
            if (message.StartsWith("/"))
            {
                MessageCommand messageCommand = new MessageCommand(message);

                if(!ChatCommandsManager.ChatCommands.TryGetValue(messageCommand.Command.Substring(1), out ChatCommand command))
                    return false;

                List<string> arguments = messageCommand.Arguments;

                if(arguments.Count > command.Parameters.Count)
                {
                    OCCM.LogMessage($"Command {command.Name} retrieved too many arguments from user.");
                    return false;
                }

                return true;
            }

            return false;
        }

        public static List<string> Tokenize(string input)
        {
            var tokens = new List<string>();
            var current = new StringBuilder();

            bool inQuotes = false;
            char quoteChar = '\0';
            bool escape = false;

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (escape)
                {
                    // accept escaped character as-is
                    current.Append(c);
                    escape = false;
                    continue;
                }

                if (c == '\\')
                {
                    escape = true;
                    continue;
                }

                if (inQuotes)
                {
                    if (c == quoteChar)
                    {
                        inQuotes = false; // end quote
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
                else
                {
                    if (c == '"' || c == '\'')
                    {
                        inQuotes = true;
                        quoteChar = c;
                    }
                    else if (char.IsWhiteSpace(c))
                    {
                        if (current.Length > 0)
                        {
                            tokens.Add(current.ToString());
                            current.Clear();
                        }
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
            }

            if (current.Length > 0)
                tokens.Add(current.ToString());

            return tokens;
        }

        public static Dictionary<string, string> GetArgumentsForCommand(string message, IChatCommand command)
        {
            List<string> arguments = GetArgumentsFromMessage(message);

            return GetArguments(arguments, command.Parameters);
        }

        public static List<string> GetArgumentsFromMessage(string message)
        {
            List<string> tokens = Tokenize(message);

            //string commandName = tokens[0];
            List<string> arguments = tokens.Skip(1).ToList();

            return arguments;
        }

        public static Dictionary<string, string> GetArguments(List<string> arguments, Dictionary<string, (string description, string defaultValue)> parameters)
        {
            Dictionary<string, string> mappedArguments = new();
            List<string> leftoverArgs = new(arguments);
            Dictionary<string, (string desc, string def)> paramDefs = new(parameters);

            // --- 1) Parse named arguments: --param=value
            for (int i = leftoverArgs.Count - 1; i >= 0; i--)
            {
                string arg = leftoverArgs[i];

                if (arg.StartsWith("--") && arg.Contains("="))
                {
                    int eqIndex = arg.IndexOf('=');

                    string key = arg.Substring(2, eqIndex - 2);       // extract key without "--"
                    string value = arg.Substring(eqIndex + 1);         // extract value without "="

                    mappedArguments[key] = value;

                    leftoverArgs.RemoveAt(i);
                    paramDefs.Remove(key);
                }
            }

            // --- 2) Assign positional arguments (remaining)
            int pos = 0;
            foreach (var kvp in paramDefs)
            {
                string key = kvp.Key;

                if (pos < leftoverArgs.Count)
                    mappedArguments[key] = leftoverArgs[pos];
                else
                    mappedArguments[key] = kvp.Value.def; // use default if no more args

                pos++;
            }

            return mappedArguments;
        }
    }
}
