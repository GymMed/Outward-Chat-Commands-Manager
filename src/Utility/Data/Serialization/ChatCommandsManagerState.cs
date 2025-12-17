using OutwardChatCommandsManager.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Data.Serialization
{
    public class ChatCommandsManagerState
    {
        public List<ChatCommandInvocation> History;
        public Dictionary<HotkeySlots, ChatCommandInvocation> LockedHotKeys;

        public ChatCommandsManagerState(List<ChatCommandInvocation> history = null, Dictionary<HotkeySlots, ChatCommandInvocation> lockedHotKeys = null)
        {
            History = history;
            LockedHotKeys = lockedHotKeys;
        }

        public ChatCommandsManagerState(IReadOnlyList<ChatCommandInvocation> history = null, Dictionary<HotkeySlots, ChatCommandInvocation> lockedHotKeys = null)
        {
            History = history.ToList();
            LockedHotKeys = lockedHotKeys;
        }

        public ChatCommandsManagerState()
        {
            History = null;
            LockedHotKeys = null;
        }
    }
}
