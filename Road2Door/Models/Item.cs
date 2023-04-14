﻿using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public int Quantity { get; set; }

    public double Price { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Rider> Riders { get; } = new List<Rider>();
}
