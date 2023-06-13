using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int MenuId { get; set; }

    public int ItemId { get; set; }

    public int RiderId { get; set; }

    public int ConsumerId { get; set; }

    public int Quantity { get; set; }

    public virtual Consumer Consumer { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual MenueMaster Menu { get; set; } = null!;

    public virtual Rider Rider { get; set; } = null!;
}
