using NodeCanvas.Tasks.Actions;
using OutwardChatCommandsManager.Managers;
using OutwardChatCommandsManager.Utility.Data;
using OutwardChatCommandsManager.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Enums
{
    public enum HotkeySlots
    {
        Slot1,
        Slot2,
        Slot3,
    }

    public static class HotkeySlotsHelper
    {
        private static readonly Dictionary<HotkeySlots, ChatCommandInvocation> _locked
            = new();

        public static Dictionary<HotkeySlots, ChatCommandInvocation> Locked { get { return _locked; } }

        public static void Lock(HotkeySlots slot, ChatCommandInvocation invocation)
        {
#if DEBUG
            CharacterHelpers.BroadCastChatMessageToLocalCharacters($"[LOCK] Slot={slot}, Msg={invocation?.Message}");
#endif
            _locked[slot] = invocation;
        }

        public static void Release(HotkeySlots slot)
        {
            _locked.Remove(slot);
        }

        public static bool TryGet(HotkeySlots slot, out ChatCommandInvocation invocation)
        {
#if DEBUG
            CharacterHelpers.BroadCastChatMessageToLocalCharacters($"[TRYGET] Slot={slot}, Locked={_locked.ContainsKey(slot)}");
#endif
            return _locked.TryGetValue(slot, out invocation);
        }
    }
}
