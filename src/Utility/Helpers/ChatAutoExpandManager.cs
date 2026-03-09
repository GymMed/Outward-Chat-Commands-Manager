using System.Collections.Generic;

namespace OutwardChatCommandsManager.Utility.Helpers
{
    public static class ChatAutoExpandManager
    {
        public const int EXPANDED_MAX_MESSAGES = int.MaxValue;

        private static bool _isEnabled = true;
        private static int _defaultMaxMessages = 30;
        private static readonly Dictionary<ChatPanel, int> _originalMaxMessages = new();

        public static bool IsEnabled => _isEnabled;

        public static void Enable()
        {
            _isEnabled = true;
        }

        public static void Disable()
        {
            _isEnabled = false;
        }

        public static void Toggle()
        {
            _isEnabled = !_isEnabled;
        }

        public static int GetDefaultMaxMessages()
        {
            return _defaultMaxMessages;
        }

        public static void SetDefaultMaxMessages(int value)
        {
            _defaultMaxMessages = value;
        }

        public static void StoreOriginalMaxMessage(ChatPanel panel)
        {
            if (panel == null || _originalMaxMessages.ContainsKey(panel))
                return;

            _originalMaxMessages[panel] = panel.MaxMessageCount;
        }

        public static void StoreOriginalAtCommandStart(ChatPanel panel)
        {
            if (panel == null)
                return;

            if (panel.MaxMessageCount != EXPANDED_MAX_MESSAGES)
            {
                _originalMaxMessages[panel] = panel.MaxMessageCount;
            }
        }

        public static void RestoreOriginalMaxMessage(ChatPanel panel)
        {
            if (panel == null || !_originalMaxMessages.TryGetValue(panel, out int original))
                return;

            panel.MaxMessageCount = original;
            _originalMaxMessages.Remove(panel);
        }

        public static void ExpandForSystemMessage(ChatPanel panel)
        {
            if (!_isEnabled || panel == null)
                return;

            panel.MaxMessageCount = EXPANDED_MAX_MESSAGES;
        }

        public static void ResetForUserMessage(ChatPanel panel)
        {
            if (panel == null)
                return;

            RestoreOriginalMaxMessage(panel);
        }

        public static void ResetForTimer(ChatPanel panel)
        {
            if (panel == null)
                return;

            RestoreOriginalMaxMessage(panel);
        }

        public static void CleanupBeforeNewCommand(ChatPanel panel)
        {
            if (!_isEnabled || panel == null)
                return;

            int normalLimit = _defaultMaxMessages;
            if (_originalMaxMessages.TryGetValue(panel, out int stored))
            {
                normalLimit = stored;
            }

            int currentCount = panel.m_messageArchive.Count;
            int toDelete = currentCount - normalLimit;

            for (int i = 0; i < toDelete; i++)
            {
                if (panel.m_messageArchive.Count <= normalLimit)
                    break;

                ChatEntry oldest = panel.m_messageArchive[panel.m_messageArchive.Count - 1];
                if (oldest != null)
                {
                    panel.m_messageArchive.RemoveAt(panel.m_messageArchive.Count - 1);
                    UnityEngine.Object.Destroy(oldest.gameObject);
                }
            }

            panel.MaxMessageCount = EXPANDED_MAX_MESSAGES;
        }

        public static void RemovePanel(ChatPanel panel)
        {
            _originalMaxMessages.Remove(panel);
        }
    }
}
