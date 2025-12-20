using OutwardChatCommandsManager.Utility.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OutwardChatCommandsManager.Utility.Data.Serialization
{
    public class ChatCommandInvocationData
    {
        [XmlElement("CharacterUID")]
        [DefaultValue(null)]
        public UID CharacterUID { get; set; } = null;

        [XmlElement("Command")]
        [DefaultValue(null)]
        public ChatCommandData Command { get; set; } = null;

        [XmlArray("Arguments")]
        [XmlArrayItem("Argument")]
        [DefaultValue(null)]
        public List<ArgumentData> Arguments { get; set; } = null;

        [XmlElement("Message")]
        [DefaultValue(null)]
        public string Message { get; set; } = null;

        [XmlElement("Executor")]
        [DefaultValue(null)]
        public string ExecutorName { get; set; } = null;

        // Helper property to get enum
        [XmlIgnore]
        public CommandExecutors? Executor =>
            string.IsNullOrWhiteSpace(ExecutorName)
                ? (CommandExecutors?)null
                : Enum.TryParse<CommandExecutors>(ExecutorName, out var e) ? e : null;
    }
}
