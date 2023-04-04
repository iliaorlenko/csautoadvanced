using System.Collections.Generic;
using System.Xml.Serialization;
using TestConfiguration.Enums;

namespace TestConfiguration.DAL
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
