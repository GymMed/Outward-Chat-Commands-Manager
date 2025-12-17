using OutwardChatCommandsManager.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Helpers
{
    public static class CharacterHelpers
    {
        public static void BroadCastChatMessageToLocalCharacters(string message, ChatLogStatus status = ChatLogStatus.Info)
        {
            Character firstChar = CharacterManager.Instance.GetFirstLocalCharacter();

            if(firstChar?.CharacterUI?.ChatPanel != null)
            {
                ChatHelpers.SendChatLog(firstChar.CharacterUI.ChatPanel, message, status);
            }

            Character secondChar = CharacterManager.Instance.GetSecondLocalCharacter();

            if(secondChar?.CharacterUI?.ChatPanel != null)
            {
                ChatHelpers.SendChatLog(secondChar.CharacterUI.ChatPanel, message, status);
            }
        }
    }
}
