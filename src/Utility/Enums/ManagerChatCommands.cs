using OutwardChatCommandsManager.Commands;
using OutwardChatCommandsManager.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Enums
{
    public enum ManagerChatCommands
    {
        Help,
        Commands,
        LockKey,
        ReleaseKey,
        ShowCommandsHistory,
        AutoMaxChatSize,
    }

    public static class ManagerChatCommandsHelper
    {
        public static readonly Dictionary<ManagerChatCommands, ChatCommand> Commands = new()
        {
            { ManagerChatCommands.Help, new ChatCommand("help", new() {
                    {"command", 
                        ("name of command like \"toggleDebug\".", "") 
                    },
                }, "Provides details about command. Ex.:\"/help toggleDebug\".", false, (Character character, Dictionary<string, string> parameters) => {  ChatHelpers.ChatLogChatCommand(character, parameters); } )
            },
            { ManagerChatCommands.Commands, new ChatCommand("commands", new() {}, 
              "Provides details about command. Ex.:\"/help toggleDebug\".", false, (Character character, Dictionary<string, string> parameters) => {  ChatHelpers.LogAllCommandsToChat(character); } )
            },
            { ManagerChatCommands.LockKey, new ChatCommand("lockKey", new() {
                    {"index", 
                        ("Required. Key number example \"1\".", "1") 
                    },
                    {"show", 
                        ("Optional. Shows which command is already locked instead of locking it. Ex.:\".lockKey 1 show\".", "") 
                    },
                }, "Locks last command with arguments to provided Chat Commands Manager key. Ex.:\"/lockKey 1\".", false, (Character character, Dictionary<string, string> parameters) => {  ChatHelpers.LockKey(character, parameters); } )
            },
            { ManagerChatCommands.ReleaseKey, new ChatCommand("releaseKey", new() {
                    {"index", 
                        ("Required. Key number example \"1\".", "1") 
                    },
                }, "Releases command from provided Chat Commands Manager key. Ex.:\"/releaseKey 1\".", false, (Character character, Dictionary<string, string> parameters) => {  ChatHelpers.ReleaseKey(character, parameters); } )
            },
            { ManagerChatCommands.ShowCommandsHistory, new ChatCommand("history", new() {
                    {"detailed", 
                        ("Optional. Provides commands with arguments used example \"/history detailed\".", "") 
                    },
                }, "Show used commands history. Ex.:\"/history\".", false, (Character character, Dictionary<string, string> parameters) => { ChatHelpers.ShowCommandsHistory(character, parameters); } )
            },
            { ManagerChatCommands.AutoMaxChatSize, new ChatCommand("autoMaxChatSize", new() {
                    {"action", 
                        ("Optional. Can be 'on', 'off', 'toggle', or 'show'. Default: 'show'.", "show") 
                    },
                }, "Automatically expands chat history during command output. System messages kept, user messages restore limit. Default: on. Timer resets after 3 seconds of no system messages.", false, (Character character, Dictionary<string, string> parameters) => { ChatHelpers.SetAutoMaxChatSize(character, parameters); } )
            },
        };
    }
}
