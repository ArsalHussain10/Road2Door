using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class MenuDetail
{
    public int Srno { get; set; }

    public int MenueId { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

     public virtual Item Item { get; set; } = null!;
    //public virtual ICollection<Item> Items { get; set; } = new List<Item>();


    public virtual MenueMaster Menue { get; set; } = null!;
}
