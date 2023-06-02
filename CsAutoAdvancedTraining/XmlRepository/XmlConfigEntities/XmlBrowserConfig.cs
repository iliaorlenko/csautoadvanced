using System.Xml.Serialization;
using Repository.Enums;

namespace Xml.XmlConfigEntities
{
    public class XmlBrowserConfig
    {
        [XmlAttribute("name")]
        public Browser BrowserName { get; set; }

        [XmlAttribute("version")]
        public string BrowserVersion { get; set; }

        [XmlElement("user")]
        public List<XmlUser> Users { get; set; }
    }
}
