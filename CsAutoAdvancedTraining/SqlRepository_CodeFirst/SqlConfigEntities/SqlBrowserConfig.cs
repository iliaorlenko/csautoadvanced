using Repository.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlRepository_CodeFirst.SqlConfigEntities
{
    public class SqlBrowserConfig
    {
        public int Id { get; set; }

        public Browser BrowserName { get; set; }

        public string BrowserVersion { get; set; }

        public List<SqlUser> Users { get; set; }
    }
}
