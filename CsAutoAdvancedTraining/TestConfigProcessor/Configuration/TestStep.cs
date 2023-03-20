using System.Xml.Serialization;

namespace TestConfigProcessor.Configuration
{
    public class TestStep
    {
        [XmlAttribute("number")]
        public int StepNumber { get; set; }

        [XmlText]
        public string StepText { get; set; }
    }
}
