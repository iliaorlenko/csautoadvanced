using System.Collections.Generic;

namespace Repository.Models
{
    public class Config
    {
        public string ConfigName { get; set; }

        public List<BrowserConfig> BrowserConfigs { get; set;  }
    }
}
