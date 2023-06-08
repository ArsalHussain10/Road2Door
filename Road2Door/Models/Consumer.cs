﻿using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class Consumer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ConsumerLocation? ConsumerLocation { get; set; }
}
