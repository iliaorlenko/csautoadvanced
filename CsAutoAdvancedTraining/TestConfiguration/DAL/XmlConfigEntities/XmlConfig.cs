using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TestConfiguration.DAL
{
    [XmlRoot("config")]
    public class XmlConfig 
    {
        [XmlIgnore]
        public string ConfigName { get; set; }

        [XmlElement("browser")]
        public List<XmlBrowserConfig> BrowserConfigs { get; set;  }
    }
}
