using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class Notification
{
    public int RiderId { get; set; }

    public int ConsumerId { get; set; }

    public DateTime InsertionTime { get; set; }

    public virtual Consumer Consumer { get; set; } = null!;

    public virtual Rider Rider { get; set; } = null!;
}
