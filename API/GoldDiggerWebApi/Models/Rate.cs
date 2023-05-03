using System;
using System.Collections.Generic;

namespace GoldDiggerWebApi.Models;

public partial class Rate
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public decimal Rates { get; set; }
}
