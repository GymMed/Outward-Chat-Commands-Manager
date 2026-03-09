using HarmonyLib;
using OutwardChatCommandsManager.Utility.Data.Executors;
using OutwardChatCommandsManager.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Patches
{
    [HarmonyPatch(typeof(ChatPanel), nameof(ChatPanel.ChatMessageReceived))]
    public static class Patch_ChatPanel_ChatMessageReceived
    {
        const float AUTO_RESET_TIME = 3f;

        static void Prefix(ChatPanel __instance, string _senderCharUID)
        {
            bool isSystemMessage = _senderCharUID == "System";
            bool isProcessing = ChatExecutionGuard.IsProcessingCommand;

            if (!isProcessing)
                return;

            float elapsed = UnityEngine.Time.time - ChatExecutionGuard.CommandStartTime;
            if (elapsed > AUTO_RESET_TIME)
            {
                ChatAutoExpandManager.ResetForTimer(__instance);
                ChatExecutionGuard.IsProcessingCommand = false;
                return;
            }

            if (isSystemMessage)
            {
                ChatAutoExpandManager.ExpandForSystemMessage(__instance);
            }
            else
            {
                ChatAutoExpandManager.ResetForUserMessage(__instance);
                ChatExecutionGuard.IsProcessingCommand = false;
            }
        }
    }
}
