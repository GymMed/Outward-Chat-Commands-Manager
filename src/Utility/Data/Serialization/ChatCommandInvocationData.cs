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
        [XmlElement("UID")]
        [DefaultValue(null)]
        public UID UID { get; set; } = null;

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
    }
}
