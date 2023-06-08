using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class ConsumerLocation
{
    public int ConsumerId { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public virtual Consumer Consumer { get; set; } = null!;
}
