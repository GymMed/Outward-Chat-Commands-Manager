using OutwardChatCommandsManager.Commands;
using OutwardChatCommandsManager.Utility.Data;
using OutwardChatCommandsManager.Utility.Data.Executors;
using OutwardChatCommandsManager.Utility.Data.Serialization;
using OutwardChatCommandsManager.Utility.Enums;
using OutwardChatCommandsManager.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OutwardChatCommandsManager.Managers
{
    public class ChatCommandsSerializer
    {
        private static ChatCommandsSerializer _instance;

        private ChatCommandsSerializer()
        {
            ConfigPath = Path.Combine(OutwardModsCommunicator.Managers.PathsManager.ConfigPath, "Chat_Commands_Manager");
            XmlFilePath = Path.Combine(this.ConfigPath, "ChatCommandsManagerState.xml");
        }

        public static ChatCommandsSerializer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ChatCommandsSerializer();

                return _instance;
            }
        }

        public string ConfigPath { get => _configPath; set => _configPath = value; }
        public string XmlFilePath { get => _xmlFilePath; set => _xmlFilePath = value; }

        private string _configPath = "";
        private string _xmlFilePath = "";

        public ChatCommandsManagerStateFile Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    OCCM.LogMessage($"Chat Commands Manager file not found at: {path}");
                    return null;
                }

                XmlSerializer serializer = new(typeof(ChatCommandsManagerStateFile));

                using FileStream fs = new(path, FileMode.Open, FileAccess.Read);
                return serializer.Deserialize(fs) as ChatCommandsManagerStateFile;
            }
            catch (Exception ex)
            {
                OCCM.LogSL($"Failed to load Chat Commands Manager file at '{path}': {ex.Message}");
                return null;
            }
        }

        public void LoadManagerState()
        {
            if (!File.Exists(XmlFilePath))
                return;

            LoadManagerState(XmlFilePath);
        }

        public void LoadManagerState(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    OCCM.LogSL($"ChatCommandsSerializer@LoadManagerState file not found at: {path}");
                    return;
                }

                ChatCommandsManagerStateFile file = this.Load(path);

                if (file == null)
                    return;

                ChatCommandsManagerState state = GetManagerState(file);
                ChatCommandsManager.Instance.RestoreHistory(state.History);

                int totalHotKeys = Enum.GetValues(typeof(HotkeySlots)).Length;

                for (int currentHotKey = 0; currentHotKey < state.LockedHotKeys.Count; currentHotKey++)
                {
                    if (totalHotKeys > currentHotKey)
                    {
                        HotkeySlots slot = (HotkeySlots)currentHotKey;

                        if(state.LockedHotKeys.TryGetValue(slot, out ChatCommandInvocation invocation))
                            HotkeySlotsHelper.Lock(slot, state.LockedHotKeys[slot]);

                        currentHotKey++;
                    }
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
                OCCM.LogMessage($"ChatCommandsSerializer@LoadManagerState failed loading '{path}': {ex.Message}");
            }
        }

        public ChatCommandsManagerState GetChatManagerState()
        {
            ChatCommandsManagerState state = new(ChatCommandsManager.History.Items, HotkeySlotsHelper.Locked);
            return state;
        }

        public void AutomaticSaveManagerToXml()
        {
            //string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            //string xmlFilePath = Path.Combine(ConfigPath, "ChatCommandsManager-" + timestamp + ".xml");

            SaveManagerStateToXml(XmlFilePath, GetChatManagerState());
        }

        public void SaveManagerStateToXml(string filePath, ChatCommandsManagerState state)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var file = BuildManagerStateFile(state);

                var serializer = new XmlSerializer(typeof(ChatCommandsManagerStateFile));

                var xmlWriterSettings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineOnAttributes = false
                };

                using (var writer = XmlWriter.Create(filePath, xmlWriterSettings))
                {
                    serializer.Serialize(writer, file);
                }
            }
            catch (Exception ex)
            {
                OCCM.LogMessage($"ChatCommandsSerializer@SaveManagerStateToXml failed saving '{filePath}': {ex.Message}");
            }
        }

        public ChatCommandsManagerState GetManagerState(ChatCommandsManagerStateFile file)
        {
            ChatCommandsManagerState state = new();
            List<ChatCommandInvocation> history = new();
            Dictionary<HotkeySlots, ChatCommandInvocation> hotKeys = new();

            foreach(ChatCommandInvocationData invocationData in file.History)
            {
                if (!GetInvocationFromData(invocationData, out ChatCommandInvocation invocation))
                    continue;

                history.Add(invocation);
            }

            foreach(HotkeyBindingData hotKeyData in file.HotkeyBindings)
            {
                if(!GetInvocationFromData(hotKeyData.Invocation, out ChatCommandInvocation invocation))
                {
                    continue;
                }

                if (!hotKeyData.Slot.HasValue)
                    continue;

                hotKeys.Add(hotKeyData.Slot.Value, invocation);
            }

            state.History = history;
            state.LockedHotKeys = hotKeys;

            return state;
        }

        public bool GetInvocationFromData(ChatCommandInvocationData invocationData, out ChatCommandInvocation invocation)
        {
            invocation = null;
            ChatCommandData commandData = invocationData.Command;
            if (!ChatCommandsManager.ChatCommands.TryGetValue(commandData.Name, out ChatCommand retrievedCommand))
            {
                OCCM.LogMessage($"ChatCommandsSerializer@GetManagerState command {commandData.Name} not found!");
                return false;
            }

            Character character = CharacterManager.Instance.GetCharacter(invocationData.CharacterUID);

            if(character == null)
            {
                OCCM.LogMessage($"ChatCommandsSerializer@GetManagerState character not found!");
                return false;
            }

            invocation = new(character, retrievedCommand, GetArgumentsFromInvocationData(invocationData.Arguments), new ManagedCommandExecutor());
            return true;
        }

        public Dictionary<string, string> GetArgumentsFromInvocationData(List<ArgumentData> argumentsData)
        {
            Dictionary<string, string> arguments = new();

            foreach(ArgumentData argumentData in argumentsData)
            {
                arguments.Add(argumentData.Parameter, argumentData.Argument);
            }

            return arguments;
        }

        public ChatCommandsManagerStateFile BuildManagerStateFile(ChatCommandsManagerState state)
        {
            var file = new ChatCommandsManagerStateFile
            {
                History = new List<ChatCommandInvocationData>(),
                HotkeyBindings = new List<HotkeyBindingData>()
            };

            foreach (ChatCommandInvocation invocation in state.History)
            {
                file.History.Add(GetInvocationData(invocation));
            }

            foreach(var hotKey in state.LockedHotKeys)
            {
                var hotKeyBindingData = new HotkeyBindingData
                {
                    SlotName = hotKey.Key.ToString() ?? "",
                    Invocation = GetInvocationData(hotKey.Value)
                };

                file.HotkeyBindings.Add(hotKeyBindingData);
            }

            return file;
        }

        public ChatCommandInvocationData GetInvocationData(ChatCommandInvocation invocation)
        {
            var command = invocation.Command;

            var commandData = new ChatCommandData
            {
                Name = command.Name,
                //Description = command.Description,
                //IsCheat = command.IsCheat,
                //RequireDebugMode = command.RequireDebugMode,
                //Parameters = new()
            };

            /*
            foreach (var parameter in invocation.Command.Parameters)
            {
                var parameterData = new ParameterDefinitionData
                {
                    Name = parameter.Key,
                    Description = parameter.Value.Item1,
                    DefaultValue = parameter.Value.Item2,
                };

                commandData.Parameters.Add(parameterData);
            }
            */

            // Convert ChatCommandInvocation -> ChatCommandInvocationData 
            var invocationData = new ChatCommandInvocationData
            {
                //UID = invocation.UID,
                CharacterUID = invocation.Character.UID,
                Command = commandData,
                Arguments = new()
            };

            foreach (var argument in invocation.Arguments)
            {
                var argumentData = new ArgumentData
                {
                    Parameter = argument.Key,
                    Argument = argument.Value
                };

                invocationData.Arguments.Add(argumentData);
            }

            return invocationData;
        }
    }
}
