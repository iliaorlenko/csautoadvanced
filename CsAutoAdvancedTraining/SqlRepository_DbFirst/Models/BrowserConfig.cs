using System;
using System.Collections.Generic;

namespace SqlRepository_DbFirst.Models;

public partial class BrowserConfig
{
    public int Id { get; set; }

    public string BrowserName { get; set; } = null!;

    public string BrowserVersion { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
