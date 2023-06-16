using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class OrderNotification
{
    public int Sr { get; set; }

    public int OrderId { get; set; }

    public int RiderId { get; set; }

    public int View { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Rider Rider { get; set; } = null!;
}
