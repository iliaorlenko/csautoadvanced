using System.Collections.Generic;
using System.Xml.Serialization;

namespace TestConfigProcessor.Configuration
{
    public class Test
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("expected")]
        public string ExpectedResult { get; set; }

        [XmlArray("steps")]
        [XmlArrayItem("step", typeof(TestStep))]
        public List<TestStep> Steps { get; set; }
    }
}
