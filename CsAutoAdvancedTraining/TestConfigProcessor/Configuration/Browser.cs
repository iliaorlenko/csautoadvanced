using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using TestConfigProcessor.Enums;

namespace TestConfigProcessor.Configuration
{
    public class Browser
    {
        [XmlAttribute("name")]
        public BrowserType Name { get; set; }

        [XmlAttribute("version")]
        public string BrowserVersion { get; set; }

        [XmlElement("user")]
        public List<User> Users { get; set; }
    }
}
