using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OutwardChatCommandsManager.Utility.Data.Serialization
{
    [XmlRoot("ChatCommandsManager")]
    public class ChatCommandsManagerStateFile
    {
        [XmlArray("History")]
        [XmlArrayItem("ChatCommandInvocation")]
        [DefaultValue(null)]
        public List<ChatCommandInvocationData> History { get; set; } = null;

        [XmlArray("HotkeyBindings")]
        [XmlArrayItem("Hotkey")]
        [DefaultValue(null)]
        public List<HotkeyBindingData> HotkeyBindings { get; set; } = null;
    }
}
