using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class InventoryItem
{
    public int RiderId { get; set; }

    public int ItemId { get; set; }

    public int Srno { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Rider Rider { get; set; } = null!;
}
