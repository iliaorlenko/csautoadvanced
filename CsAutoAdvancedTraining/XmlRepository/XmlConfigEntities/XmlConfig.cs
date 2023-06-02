using System.Xml.Serialization;

namespace Xml.XmlConfigEntities
{
    [XmlRoot("config")]
    public class XmlConfig
    {
        [XmlElement("browser")]
        public List<XmlBrowserConfig> BrowserConfigs { get; set;  }
    }
}
