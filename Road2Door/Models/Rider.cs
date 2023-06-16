using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class Rider
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Cnic { get; set; } = null!;

    public string? Phone { get; set; }

    public int? Status { get; set; }

    public string License { get; set; } = null!;

    public string PoliceRecord { get; set; } = null!;

    public virtual ICollection<InventoryItem> InventoryItems { get; } = new List<InventoryItem>();

    public virtual ICollection<MenueMaster> MenueMasters { get; } = new List<MenueMaster>();

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<OrderNotification> OrderNotifications { get; } = new List<OrderNotification>();

    public virtual RiderLocation? RiderLocation { get; set; }
}
