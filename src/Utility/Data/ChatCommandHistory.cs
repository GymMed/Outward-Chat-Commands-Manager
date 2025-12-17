using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardChatCommandsManager.Utility.Data
{
    public class ChatCommandHistory
    {
        private int _maxSize;
        private readonly List<ChatCommandInvocation> _items = new();

        public IReadOnlyList<ChatCommandInvocation> Items => _items;

        public int MaxSize { get => _maxSize; set => _maxSize = value; }

        public ChatCommandHistory(int maxSize = 10)
        {
            MaxSize = maxSize;
        }

        public ChatCommandHistory()
        {
            MaxSize = OCCM.HistorySize.Value;
        }

        public ChatCommandHistory(List<ChatCommandInvocation> oldHistory)
        {
            MaxSize = OCCM.HistorySize.Value;
            _items = new List<ChatCommandInvocation>(oldHistory);
        }

        public void Add(ChatCommandInvocation invocation)
        {
            if (_items.Count >= MaxSize)
                _items.RemoveAt(0);

            _items.Add(invocation);
        }

        public ChatCommandInvocation GetFirst()
        {
            return _items.Count > 0 ? _items.First() : null;
        }

        public ChatCommandInvocation GetLast()
        {
            return _items.Count > 0 ? _items.Last() : null;
        }

        public ChatCommandInvocation ElementAt(int index)
        {
            return _items.Count > index && index > -1 ? _items.ElementAt(index) : null;
        }

        public int FindPreviousUnique(int start)
        {
            if (start < 1 || start > Items.Count - 1)
                return -1;

            if (start <= 0 || start >= Items.Count)
                return start;

            string current = Items[start].Message;

            for (int i = start - 1; i >= 0; i--)
            {
                if (Items[i].Message != current)
                    return i;
            }

            return start; // no unique found
        }
    }
}
