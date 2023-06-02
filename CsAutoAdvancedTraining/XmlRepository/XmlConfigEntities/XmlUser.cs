using System.Collections.Generic;
using System.Xml.Serialization;
using Repository.Enums;
using Repository.Models;

namespace Xml.XmlConfigEntities
{
    public class XmlUser
    {
        [XmlAttribute("role")]
        public UserRole Role { get; set; }

        [XmlElement("login")]
        public string Login { get; set; }

        [XmlElement("password")]
        public string Password { get; set; }

        [XmlArray("tests")]
        [XmlArrayItem("test", typeof(XmlTest))]
        public List<XmlTest> Tests { get; set; }
    }
}
