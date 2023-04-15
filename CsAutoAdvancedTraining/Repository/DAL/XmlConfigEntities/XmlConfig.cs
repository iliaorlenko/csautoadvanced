using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Repository.DAL.XmlConfigEntities
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
