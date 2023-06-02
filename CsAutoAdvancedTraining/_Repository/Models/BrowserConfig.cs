using System.Collections.Generic;
using Repository.Enums;
using Repository.Interfaces;

namespace Repository.Models
{
    public class BrowserConfig
    {
        public Browser BrowserName { get; set; }

        public string BrowserVersion { get; set; }

        public List<User> Users { get; set; }
    }
}
