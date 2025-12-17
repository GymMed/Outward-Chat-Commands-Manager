using OutwardChatCommandsManager.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Enums
{
    public enum OriginalChatCommands
    {
        ToggleDebug,
        Teleport,
        Defeat,
        Weather
    }

    public static class OriginalChatCommandsHelper
    {
        public static readonly Dictionary<OriginalChatCommands, OriginalChatCommand> Commands = new()
        {
            { OriginalChatCommands.ToggleDebug, new OriginalChatCommand("toggleDebug", new() { { "status", ("Enable or disable debug mode. Can be provided as on or off, everything else is ignored.", null) } }, "Turns on Debug mode. Ex.:\"/toggleDebug on\"", true, false) },
            { OriginalChatCommands.Teleport, new OriginalChatCommand("tp", new() { { "position", ("Teleport position.", null) } }, "Tries to teleport player to provided position. Ex.:\"/tp 12.5,0,-4\"", true, true) },
            { OriginalChatCommands.Defeat, new OriginalChatCommand("defeat", new() { 
                { "subcommand", ("One of: \nlist - provides list of deafeats | \nforce - forces defeat scenario | \nclear - restores normal defeat behaviour.", null) },
                { "id", ("Defeat ID (required for 'force')", "") },
            }, "Defeat scenario managing command. Ex.:\"/defeat list\"", true, true) },
            { OriginalChatCommands.Weather, new OriginalChatCommand("weather", new() { 
                { "id", ("Weather ID index(0..n)", "0") },
                { "instant", ("If true, apply change immediately (true/false)", "true") },
            }, "Checks/sets the weather. Ex.:\"/weather 1 true\"", true, true) },
        };

        public static readonly Dictionary<string, OriginalChatCommand> CommandsByName =
            Commands.Values.ToDictionary(
                c => c.Name,
                c => c,
                StringComparer.InvariantCultureIgnoreCase
            );
    }
}
