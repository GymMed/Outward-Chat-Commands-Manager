using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SideLoader;
using OutwardModsCommunicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using OutwardModsCommunicator.EventBus;
using OutwardChatCommandsManager.Utility.Enums;
using OutwardChatCommandsManager.Commands;
using OutwardChatCommandsManager.Managers;
using OutwardChatCommandsManager.Utility.Helpers;
using OutwardChatCommandsManager.Utility.Data;
using OutwardChatCommandsManager.Events;

namespace OutwardChatCommandsManager
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(SideLoader.SL.GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(OutwardModsCommunicator.OMC.GUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class OCCM : BaseUnityPlugin
    {
        // Choose a GUID for your project. Change "myname" and "mymodpack".
        public const string GUID = "gymmed.chat_commands_manager";
        // Choose a NAME for your project, generally the same as your Assembly Name.
        public const string NAME = "Chat Commands Manager";
        // Increment the VERSION when you release a new version of your mod.
        public const string VERSION = "0.0.2";

        // Choose prefix for log messages for quicker search and readablity
        public static string prefix = "[Chat-Commands-Manager]";

        // Will be used as id for accepting events from other mods 
        public const string EVENTS_LISTENER_GUID = GUID + "_*";

        public const string COMMAND_KEY = "Execute last/locked command";

        internal static ManualLogSource Log;

        // If you need settings, define them like so:
        //public static ConfigEntry<bool> ExampleConfig;
        public static ConfigEntry<int> HistorySize;

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            // You can find BepInEx logs in directory "BepInEx\LogOutput.log"
            Log = this.Logger;
            LogMessage($"Hello world from {NAME} {VERSION}!");

            HistorySize = this.Config.Bind(
                "Commands History",
                "HistorySize",
                10,
                "How many previously typed commands should be kept in history?"
            );

            HistorySize.SettingChanged += (sender, args) =>
            {
                ChatCommandsManager.History.MaxSize = HistorySize.Value;
            };

            int totalKeys = Enum.GetValues(typeof(HotkeySlots)).Length;
            for (int currentKey = 0; currentKey < totalKeys; currentKey++)
            {
                CustomKeybindings.AddAction(COMMAND_KEY + " " + (currentKey + 1), KeybindingsCategory.CustomKeybindings, ControlType.Both);
            }

            // Harmony is for patching methods. If you're not patching anything, you can comment-out or delete this line.
            new Harmony(GUID).PatchAll();

            EventBusRegister.RegisterEvents();
            EventBusSubscriber.AddSubscribers();

            SL.OnSceneLoaded += () =>
            {
                Character first = CharacterManager.Instance.GetFirstLocalCharacter();
                if (first != null)
                    ChatCommandsSerializer.Instance.LoadManagerState(first);

                Character second = CharacterManager.Instance.GetSecondLocalCharacter();
                if (second != null)
                    ChatCommandsSerializer.Instance.LoadManagerState(second);
            };
        }

        // Update is called once per frame. Use this only if needed.
        // You also have all other MonoBehaviour methods available (OnGUI, etc)
        internal void Update()
        {
            int totalKeys = Enum.GetValues(typeof(HotkeySlots)).Length;
            int totalNotLockedKeys = 0;

            for (int currentKey = 0; currentKey < totalKeys; currentKey++)
            {
                HotkeySlots slot = (HotkeySlots)currentKey;

                if (!HotkeySlotsHelper.TryGet(slot, out var invocation))
                {
                    totalNotLockedKeys++;
                }

                if (!CustomKeybindings.GetKeyDown(COMMAND_KEY + " " + (currentKey + 1)))
                    continue;

                if (invocation != null)
                {
                    invocation.TriggerFunction();
                    continue;
                }

                var items = ChatCommandsManager.History.Items;
                var itemsCount = items.Count();

                // optional fallback
                if (itemsCount > totalNotLockedKeys)
                {
                    var last = items[itemsCount - totalNotLockedKeys];

                    if (last != null)
                    {
                        last.TriggerFunction();
                        continue;
                    }
                }

                CharacterHelpers.BroadCastChatMessageToLocalCharacters(
                    $"Could not retrieve command for {currentKey + 1} hotkey.",
                    ChatLogStatus.Warning);
            }
        }

        //  Log message with prefix
        public static void LogMessage(string message)
        {
            Log.LogMessage($"{OCCM.prefix} {message}");
        }

        // Log message through side loader, helps to see it
        // if you are using UnityExplorer and want to see live logs
        public static void LogSL(string message)
        {
            SL.Log($"{OCCM.prefix} {message}");
        }

        // Gets mod dll location
        public static string GetProjectLocation()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        // This is an example of a Harmony patch.
        // If you're not using this, you should delete it.
        [HarmonyPatch(typeof(ResourcesPrefabManager), nameof(ResourcesPrefabManager.Load))]
        public class ResourcesPrefabManager_Load
        {
            static void Postfix(ResourcesPrefabManager __instance)
            {
                // use Debug build for things you don't want to release
                #if DEBUG
                // provide class and method separated by @ for easier live debugging
                LogSL("ResourcesPrefabManager@Load called!");
                #endif

                foreach(KeyValuePair<ManagerChatCommands, ChatCommand> command in ManagerChatCommandsHelper.Commands)
                {
                    ChatCommandsManager.Instance.AddChatCommand(command.Value);
                }
            }
        }
    }
}
