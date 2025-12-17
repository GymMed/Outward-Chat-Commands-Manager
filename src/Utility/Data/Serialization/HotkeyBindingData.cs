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
    public class HotkeyBindingData
    {
        [XmlElement("Slot")]
        [DefaultValue(null)]
        public string SlotName { get; set; } = "";

        [XmlElement("Invocation")]
        [DefaultValue(null)]
        public ChatCommandInvocationData Invocation { get; set; } = null;

        // Helper property to get enum
        [XmlIgnore]
        public HotkeySlots? Slot =>
            string.IsNullOrWhiteSpace(SlotName)
                ? (HotkeySlots?)null
                : Enum.TryParse<HotkeySlots>(SlotName, out var s) ? s : null;
    }
}
