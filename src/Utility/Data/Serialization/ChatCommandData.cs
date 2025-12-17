using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OutwardChatCommandsManager.Utility.Data.Serialization
{
    public class ChatCommandData
    {
        [XmlElement("Name")]
        [DefaultValue("")]
        public string Name { get; set; } = "";

        /*
        [XmlArray("Parameters")]
        [XmlArrayItem("Parameter")]
        [DefaultValue(null)]
        public List<ParameterDefinitionData> Parameters { get; set; } = null;

        [XmlElement("Description")]
        [DefaultValue("")]
        public string Description { get; set; } = "";

        [XmlElement("IsCheat")]
        [DefaultValue(false)]
        public bool IsCheat { get; set; } = false;

        [XmlElement("RequireDebugMode")]
        [DefaultValue(false)]
        public bool RequireDebugMode { get; set; } = false;
        */
    }
}
