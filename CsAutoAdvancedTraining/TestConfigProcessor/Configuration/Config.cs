using System.Collections.Generic;
using System.Xml.Serialization;
using TestConfigProcessor.Enums;

namespace TestConfigProcessor.Configuration
{
    [XmlRoot("config")]
    public partial class Config
    {
        [XmlIgnore]
        public string Name { get; set; }

        [XmlIgnore]
        public TestConfigType Type { get; set; }

        [XmlElement("browser")]
        public List<Browser> Browsers { get; set;  }
    }
}
