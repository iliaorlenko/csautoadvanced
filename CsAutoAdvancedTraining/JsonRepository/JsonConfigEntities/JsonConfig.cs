using System.Text.Json.Serialization;
using Repository.Models;

namespace Json.JsonConfigEntities
{
    public class JsonConfig
    {
        [JsonPropertyName("Browsers")]
        public List<BrowserConfig> BrowserConfigs { get; set; }
    }
}
