using System.Collections.Generic;
using System.Text.Json.Serialization;
using TestConfiguration.Models;

namespace TestConfiguration.DAL.JsonConfigEntities
{
    public class JsonConfig
    {
        [JsonIgnore]
        public string ConfigName { get; set; }

        [JsonPropertyName("Browsers")]
        public List<BrowserConfig> BrowserConfigs { get; set; }
    }
}
