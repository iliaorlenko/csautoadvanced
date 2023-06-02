using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml.XmlConfigEntities
{
    public class XmlTest
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("expected")]
        public string ExpectedResult { get; set; }

        [XmlArray("steps")]
        [XmlArrayItem("step", typeof(XmlTestStep))]
        public List<XmlTestStep> Steps { get; set; }
    }
}
