using System;
using System.Collections.Generic;

namespace GoldDiggerWebApi.Models;

public partial class PreciousMetal
{
    public int Id { get; set; }

    public string MetalName { get; set; } = null!;

    public decimal PricePerKg { get; set; }
}
