using System;
using System.Collections.Generic;

namespace SqlRepository_DbFirst.Models;

public partial class User
{
    public int Id { get; set; }

    public string Role { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();

    public virtual ICollection<BrowserConfig> BrowserConfigs { get; set; } = new List<BrowserConfig>();
}
