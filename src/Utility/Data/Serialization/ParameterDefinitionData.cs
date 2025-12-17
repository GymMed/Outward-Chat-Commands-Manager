using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OutwardChatCommandsManager.Utility.Data.Serialization
{
    public class ParameterDefinitionData
    {
        [XmlElement("Name")]
        [DefaultValue("")]
        public string Name { get; set; } = "";

        [XmlElement("Description")]
        [DefaultValue("")]
        public string Description { get; set; } = "";

        [XmlElement("Default")]
        [DefaultValue("")]
        public string DefaultValue { get; set; } = "";
    }
}
