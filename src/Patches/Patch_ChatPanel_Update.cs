using HarmonyLib;
using OutwardChatCommandsManager.Managers;
using OutwardChatCommandsManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace OutwardChatCommandsManager.Patches
{
    [HarmonyPatch(typeof(ChatPanel), nameof(ChatPanel.Update))]
    public static class Patch_ChatPanel_Update
    {
        private static readonly ConditionalWeakTable<ChatPanel, ChatPanelHistoryState> _state
            = new();

        static void Postfix(ChatPanel __instance)
        {
            if (__instance.m_chatEntry == null)
                return;

            if (!__instance.m_chatEntry.isFocused)
                return;

            var state = _state.GetOrCreateValue(__instance);
            var input = __instance.m_chatEntry;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                HandleArrowUp(input, state);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                HandleArrowDown(input, state);
            }
            else if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Return))
            {
                ExitHistoryMode(state);
            }
        }

        static void PlayErrorSound()
        {
            Global.AudioManager.PlaySound(
                GlobalAudioManager.Sounds.SFX_IMPACT_WeaponOnFlesh3,
                0f, 1f, 1f, 1f, 1f);
        }

        static void HandleArrowUp(InputField input, ChatPanelHistoryState state)
        {
            var history = ChatCommandsManager.History.Items;
            if (history.Count == 0)
            {
                PlayErrorSound();
                return;
            }

            if (state.HistoryIndex == -1)
            {
                state.CachedLiveInput = input.text;
                state.HistoryIndex = history.Count - 1;
            }
            else
            {
                int next = ChatCommandsManager.History.FindPreviousUnique(state.HistoryIndex);

                if (next == state.HistoryIndex)
                {
                    PlayErrorSound();
                    return;
                }

                state.HistoryIndex = next;
            }

            input.text = history[state.HistoryIndex].Message;
            input.caretPosition = input.text.Length;
        }

        static void HandleArrowDown(InputField input, ChatPanelHistoryState state)
        {
            if (state.HistoryIndex == -1)
            {
                PlayErrorSound();
                return;
            }

            var history = ChatCommandsManager.History.Items;
            int start = state.HistoryIndex;
            string current = history[start].Message;

            for (int i = start + 1; i < history.Count; i++)
            {
                if (history[i].Message != current)
                {
                    state.HistoryIndex = i;
                    input.text = history[i].Message;
                    input.caretPosition = input.text.Length;
                    return;
                }
            }

            // no newer unique → exit history mode
            input.text = state.CachedLiveInput ?? "";
            ExitHistoryMode(state);
            input.caretPosition = input.text.Length;

            PlayErrorSound();
        }

        static void ExitHistoryMode(ChatPanelHistoryState state)
        {
            state.HistoryIndex = -1;
            state.CachedLiveInput = null;
        }
    }
}
