using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class OrderDetail
{
    public int Sr { get; set; }

    public int OrderId { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
