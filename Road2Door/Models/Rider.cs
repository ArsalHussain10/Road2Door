using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Road2Door.Models;

public partial class Rider
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Cnic { get; set; } = null!;

    public string? Phone { get; set; }

    public int? Status { get; set; }

    public string License { get; set; } = null!;

    public string PoliceRecord { get; set; } = null!;
}
