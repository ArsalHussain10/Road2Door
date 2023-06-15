using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int MenuId { get; set; }

    public int ConsumerId { get; set; }

    public string? Date { get; set; }

    public virtual Consumer Consumer { get; set; } = null!;

    public virtual MenueMaster Menu { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
