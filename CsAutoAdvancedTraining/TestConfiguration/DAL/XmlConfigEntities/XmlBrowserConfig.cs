using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TestConfiguration.Enums;

namespace TestConfiguration.DAL
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
