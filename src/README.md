<h1 align="center">
    Outward Chat Commands Manager
</h1>
<br/>
<div align="center">
  <img src="https://raw.githubusercontent.com/GymMed/Outward-Chat-Commands-Manager/refs/heads/main/preview/images/Logo.png" alt="Logo"/>
</div>

<div align="center">
	<a href="https://thunderstore.io/c/outward/p/GymMed/Chat_Commands_Manager/">
		<img src="https://img.shields.io/thunderstore/dt/GymMed/Chat_Commands_Manager" alt="Thunderstore Downloads">
	</a>
	<a href="https://github.com/GymMed/Outward-Chat-Commands-Manager/releases/latest">
		<img src="https://img.shields.io/thunderstore/v/GymMed/Chat_Commands_Manager" alt="Thunderstore Version">
	</a>
	<a href="https://github.com/GymMed/Outward-Mods-Communicator/releases/latest">
		<img src="https://img.shields.io/badge/Mods_Communicator-v1.2.0-D4BD00" alt="Mods Communicator Version">
	</a>
</div>

<div align="center">
	CLI brought to Outward Chat
</div>

<details>
    <summary>Deeper Explanation</summary>
<i>CLI</i> stands for <i>Command Line Interface</i>. This mod brings a CLI-like
experience to the in-game chat, allowing you to easily create and use commands
directly in chat.<br><br>

Commands can be created through the <a
href="https://thunderstore.io/c/outward/p/GymMed/Mods_Communicator/">Mods
Communicator</a> without the need to build a custom UI. This provides fast and
convenient access to dynamic mod methods through chat commands.

The Chat Commands Manager includes command history and allows navigation using
the <code>ArrowUp</code> and <code>ArrowDown</code> keys. It also provides several
helper commands and the ability to bind recently used commands to hotkeys.

As a command manager, it allows inspection of all registered commands, including
the original Outward commands. The tool also supports creating **cheat commands**
and **debug-mode–required commands**. When cheat commands are used, the current
game session will be marked as **unsavable**, preventing progress from being
saved.
</details>

## How to use it

Firstly, install [Mods
Communicator](https://github.com/GymMed/Outward-Mods-Communicator) After that,
you can publish and subscribe to <code>ChatCommandsManager</code> events.

All events are registered and visible in Mods Communicator’s logging system.
However, it is still recommended to read the event descriptions below and
review the examples.

### Built-in Commands

The most important commands are:
<code>/commands</code>, <code>/help</code>, and <code>/history</code>.

These commands provide essential information about available commands and their
usage.

For hotkey control, the following commands are available:
<code>/lockKey</code> and <code>/releaseKey</code>.
Use <code>/help lockKey</code> for detailed information about how to use these
commands.

### Events

<details>
    <summary>Publish</summary>
Mod Namespace: <code>gymmed.chat_commands_manager_*</code><br>

<details>
    <summary>Add Command</summary>
Event Name: <code>ChatCommandsManager@AddChatCommand</code><br>

Accepts all command parameters data described in the **Command Parameters** section.

<details>
    <summary>Example</summary>
This example shows a command registered during
<code>ResourcesPrefabManager.Load</code>. This is done for safety in case a soft
dependency is missing. Commands can be added or removed dynamically at any time.
<pre><code>using OutwardModsCommunicator;
...
[HarmonyPatch(typeof(ResourcesPrefabManager), nameof(ResourcesPrefabManager.Load))]
public class ResourcesPrefabManager_Load
{
    static void Postfix(ResourcesPrefabManager __instance)
    {
        Dictionary&lt;string, (string, string)&gt; myParameters = new()
        {
            {
                "myParameter",
                ("parameterDescription", defaultValue)
            }
        };
        Action&lt;Character, Dictionary&lt;string, string&gt;&gt; myFunction = MyCommandFunction;
        var payload = new EventPayload
        {
            // command is a chat message trigger /myCommandName
            ["command"] = "myCommandName",
            ["parameters"] = myParameters,
            ["function"] = myFunction,
            ["description"] = "Your command description that will be seen through /help myCommandName ."
        };
        EventBus.Publish("gymmed.chat_commands_manager_*", "ChatCommandsManager@AddChatCommand", payload);
    }
}
public static void MyCommandFunction(Character characterSender, Dictionary&lt;string, string&gt; arguments)
{
    ChatPanel panel = characterSender?.CharacterUI?.ChatPanel;
    if(panel == null)
    {
        Plugin.LogMessage("MySuperFunction Tried to use missing chatPanel.");
        return;
    }
    // send message to chat
    panel.ChatMessageReceived("System", "&lt;color=#9900cc&gt;Your command was triggered!&lt;/color&gt;");
    // parameter value = argument
    arguments.TryGetValue("myParameter", out string parameterValue);
    if(string.IsNullOrWhiteSpace(parameterValue))
    {
        ...Your code when argument is missing.
        return;
    }
    ...Your code when argument is provided.
}</code></pre>
</details>
</details>

<details>
    <summary>Remove Command</summary>
Event Name: <code>ChatCommandsManager@RemoveChatCommand</code><br>

Accepts only the following parameter:
<code>"command"</code> — the name of the command to remove.

<details>
    <summary>Example</summary>
<pre><code>using OutwardModsCommunicator;
...
[HarmonyPatch(typeof(ResourcesPrefabManager), nameof(ResourcesPrefabManager.Load))]
public class ResourcesPrefabManager_Load
{
    static void Postfix(ResourcesPrefabManager __instance)
    {
        var payload = new EventPayload
        {
            // command is a chat message trigger /myCommandName
            ["command"] = "myCommandName",
        };
        EventBus.Publish("gymmed.chat_commands_manager_*", "ChatCommandsManager@RemoveChatCommand", payload);
    }
}</code></pre>
</details>
</details>

</details>

<details>
    <summary>Subscribe</summary>
Mod Namespace: <code>gymmed.chat_commands_manager</code><br>
Allows you to listen for events triggered by the Chat Commands Manager.

<details>
    <summary>Command Added</summary>
Event Name: <code>ChatCommandsManager@AddChatCommand_After</code><br>

This event is triggered after a command has been added and passes all command
parameters back to the subscriber.

<details>
    <summary>Example</summary>
<pre><code>using OutwardModsCommunicator;
...
[HarmonyPatch(typeof(ResourcesPrefabManager), nameof(ResourcesPrefabManager.Load))]
public class ResourcesPrefabManager_Load
{
    static void Postfix(ResourcesPrefabManager __instance)
    {
        EventBus.Subscribe("gymmed.chat_commands_manager", "ChatCommandsManager@AddChatCommand_After", AddedCommand);
    }
}
public static void AddedCommand(EventPayload payload)
{
    if (payload == null) return;
    string commandName = payload.Get&lt;string&gt;("command", null);
    if(string.IsNullOrEmpty(commandName))
    {
        Log.LogMessage($"MyMod@AddedCommand didn't receive command variable!");
        return;
    }
    ...continue with execution...
}</code></pre>
</details>
</details>

<details>
    <summary>Command Removed</summary>
Event Name: <code>ChatCommandsManager@RemoveChatCommand_After</code><br>

This event is triggered after a command has been removed and passes all command
parameters back to the subscriber.
<details>
    <summary>Example</summary>
<pre><code>using OutwardModsCommunicator;
...
[HarmonyPatch(typeof(ResourcesPrefabManager), nameof(ResourcesPrefabManager.Load))]
public class ResourcesPrefabManager_Load
{
    static void Postfix(ResourcesPrefabManager __instance)
    {
        EventBus.Subscribe("gymmed.chat_commands_manager", "ChatCommandsManager@RemoveChatCommand_After", RemovedCommand);
    }
}
public static void RemovedCommand(EventPayload payload)
{
    if (payload == null) return;
    string commandName = payload.Get&lt;string&gt;("command", null);
    if(string.IsNullOrEmpty(commandName))
    {
        Log.LogMessage($"MyMod@RemovedCommand didn't receive command variable!");
        return;
    }
    ...continue with execution...
}</code></pre>
</details>
</details>

</details>

### Command Parameters

<details>
    <summary>Chat command data</summary>

<table>
    <thead>
        <tr>
            <th>Variable Key</th> <th>Type</th> <th>Description</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>command</td> <td>string</td> <td>Required. The text players type in chat (e.g. "/command") to trigger your handler.</td>
        </tr>
        <tr>
            <td>parameters</td> <td>Dictionary&lt;string, (string, string)&gt;</td> <td>Optional. Parameters with name and (description, default value) provided as dictionary.</td>
        </tr>
        <tr>
            <td>function</td> <td>Action &lt;Character, Dictionary&lt;string, string&gt;&gt;</td> <td>Required. The method to execute when the command is triggered. Character is the command caller and Dictionary stores parameter and argument(value).</td>
        </tr>
        <tr>
            <td>isCheatCommand</td> <td>bool</td> <td>Optional. Default false. Determines if player game will be saved after triggering your command.</td>
        </tr>
        <tr>
            <td>description</td> <td>string</td> <td>Optional. Default null. Provides description of command for users.</td>
        </tr>
        <tr>
            <td>debugMode</td> <td>bool</td> <td>Optional. Default false. Determines if command requires debug mode to work.</td>
        </tr>
    </tbody>
</table>
</details>

## Can I find full project example how to use this?

You can view [outward basic chat commands mod here](https://github.com/GymMed/Outward-Basic-Chat-Commands).

## How to set up

To manually set up, do the following

1. Create the directory: `Outward\BepInEx\plugins\OutwardChatCommandsManager\`.
2. Extract the archive into any directory(recommend empty).
3. Move the contents of the plugins\ directory from the archive into the `BepInEx\plugins\OutwardChatCommandsManager\` directory you created.
4. It should look like `Outward\BepInEx\plugins\OutwardChatCommandsManager\OutwardChatCommandsManager.dll`
   Launch the game.

### If you liked the mod leave a star on [GitHub](https://github.com/GymMed/Outward-Chat-Commands-Manager) it's free
