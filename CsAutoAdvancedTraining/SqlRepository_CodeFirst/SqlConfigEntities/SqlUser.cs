using Repository.Enums;
using System.ComponentModel.DataAnnotations;

namespace SqlRepository_CodeFirst.SqlConfigEntities
{
    public class SqlUser
    {
        public int Id { get; set; }

        public UserRole Role { get; set; }

        public string Login { get; set; }

        [MinLength(1)]
        public string Password { get; set; }

        public List<SqlTest> Tests { get; set; }

        public List<SqlBrowserConfig> BrowserConfigs { get; set; }
    }
}
