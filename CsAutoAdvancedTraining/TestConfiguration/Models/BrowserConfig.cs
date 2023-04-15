using System.Collections.Generic;
using TestConfiguration.Enums;

namespace TestConfiguration.Models
{
    public class BrowserConfig
    {
        public Browser BrowserName { get; set; }

        public string BrowserVersion { get; set; }

        public List<User> Users { get; set; }
    }
}
