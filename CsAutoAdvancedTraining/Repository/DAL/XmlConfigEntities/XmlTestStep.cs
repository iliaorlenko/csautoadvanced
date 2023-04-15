using System.Xml.Serialization;

namespace Repository.DAL.XmlConfigEntities
{
    public class XmlTestStep
    {
        [XmlAttribute("number")]
        public int StepNumber { get; set; }

        [XmlText]
        public string StepText { get; set; }
    }
}
