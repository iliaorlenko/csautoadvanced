using System;
using System.Collections.Generic;

namespace SqlRepository_DbFirst.Models;

public partial class Test
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string ExpectedResult { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<TestStep> TestSteps { get; set; } = new List<TestStep>();

    public virtual User User { get; set; } = null!;
}
