using System.Collections.Generic;

namespace TestConfiguration.Models
{
    public class Config
    {
        public string ConfigName { get; set; }

        public List<BrowserConfig> BrowserConfigs { get; set;  }
    }
}
