using System.Collections.Generic;
using System.Xml.Serialization;
using TestConfigProcessor.Enums;

namespace TestConfigProcessor.Configuration
{
    public class User
    {

        [XmlAttribute("role")]
        public UserRole Role { get; set; }

        [XmlElement("login")]
        public string Login { get; set; }

        [XmlElement("password")]
        public string Password { get; set; }

        [XmlArray("tests")]
        [XmlArrayItem("test", typeof(Test))]
        public List<Test> Tests { get; set; }
    }
}
