using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class RiderLocation
{
    public int RiderId { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public virtual Rider Rider { get; set; } = null!;
}
