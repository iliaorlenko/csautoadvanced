using System;
using System.Collections.Generic;

namespace SqlRepository_DbFirst.Models;

public partial class TestStep
{
    public int Id { get; set; }

    public int StepNumber { get; set; }

    public string StepText { get; set; } = null!;

    public int TestId { get; set; }

    public virtual Test Test { get; set; } = null!;
}
