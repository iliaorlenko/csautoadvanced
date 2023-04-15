using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Repository.DAL.XmlConfigEntities
{
    public class XmlTest
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("expected")]
        public string ExpectedResult { get; set; }

        [XmlArray("steps")]
        [XmlArrayItem("step", typeof(XmlTestStep))]
        public List<XmlTestStep> Steps { get; set; }
    }
}
