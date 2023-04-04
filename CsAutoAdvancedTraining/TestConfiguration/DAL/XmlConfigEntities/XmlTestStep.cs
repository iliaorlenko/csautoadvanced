using System.Xml.Serialization;

namespace TestConfiguration.DAL
{
    public class XmlTestStep
    {
        [XmlAttribute("number")]
        public int StepNumber { get; set; }

        [XmlText]
        public string StepText { get; set; }
    }
}
