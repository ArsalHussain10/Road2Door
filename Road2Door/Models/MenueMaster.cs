using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class MenueMaster
{
    public int MenueId { get; set; }

    public int RiderId { get; set; }

    public string CreationDate { get; set; } = null!;

    public string? ExpirationDate { get; set; }

    public virtual ICollection<MenuDetail> MenuDetails { get; } = new List<MenuDetail>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual Rider Rider { get; set; } = null!;
}
