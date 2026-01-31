using HarmonyLib;
using OutwardChatCommandsManager.Commands;
using OutwardChatCommandsManager.Managers;
using OutwardChatCommandsManager.Utility.Data;
using OutwardChatCommandsManager.Utility.Data.Executors;
using OutwardChatCommandsManager.Utility.Data.Interfaces;
using OutwardChatCommandsManager.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Patches
{
    [HarmonyPatch(typeof(ChatPanel), nameof(ChatPanel.SendChatMessage))]
    public static class Patch_ChatPanel_SendChatMessage
    {
        static bool Prefix(ChatPanel __instance)
        {
            if (ChatExecutionGuard.IsForwarding)
                return true;

            if (!ChatHelpers.TryGetValidCommand(__instance.m_chatEntry.text, out IChatCommand command))
                return true;

            ChatCommandInvocation invocation = null;

            if (command is OriginalChatCommand originalCommand)
            {
                invocation = new ChatCommandInvocation(__instance.LocalCharacter, command, new GameChatForwardExecutor(__instance), __instance.m_chatEntry.text);
                invocation.TriggerFunction();
                return true;
            }
            else
            {
                invocation = new ChatCommandInvocation(__instance.LocalCharacter, command, new ManagedCommandExecutor(), __instance.m_chatEntry.text);
                invocation.TriggerFunction();
            }

            return false;
        }

        static void Postfix(ChatPanel __instance)
        {
            if(__instance?.m_chatEntry == null)
            {
                return;
            }

            var state = Patch_ChatPanel_Update.State.GetOrCreateValue(__instance);
            Patch_ChatPanel_Update.ExitHistoryMode(state);

            __instance.m_chatEntry.text = "";

            if (!__instance.m_chatEntry.isFocused)
                __instance.m_chatEntry.ActivateInputField();
        }
    }
}
