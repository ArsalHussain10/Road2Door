﻿using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class MenueMaster
{
    public int MenueId { get; set; }

    public int RiderId { get; set; }

    public string CreationDate { get; set; } = null!;

    public string? ExpirationDate { get; set; }

    public virtual Rider Rider { get; set; } = null!;
}