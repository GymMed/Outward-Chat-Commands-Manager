using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OutwardChatCommandsManager.Utility.Data.Serialization
{
    public class ArgumentData
    {
        [XmlElement("Parameter")]
        [DefaultValue("")]
        public string Parameter { get; set; } = "";

        [XmlElement("Argument")]
        [DefaultValue("")]
        public string Argument { get; set; } = "";
    }
}
