using System.Collections.Generic;
using System.Text.Json.Serialization;
using Repository.Models;

namespace Repository.DAL.JsonConfigEntities
{
    public class JsonConfig
    {
        [JsonIgnore]
        public string ConfigName { get; set; }

        [JsonPropertyName("Browsers")]
        public List<BrowserConfig> BrowserConfigs { get; set; }
    }
}
