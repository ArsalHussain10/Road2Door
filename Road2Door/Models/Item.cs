using System;
using System.Collections.Generic;

namespace Road2Door.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public int Quantity { get; set; }

    public double Price { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<InventoryItem> InventoryItems { get; } = new List<InventoryItem>();

    public void Update(Item updatedItem)
    {
        // Update only the attributes that are not null or empty in the updated item
        if (!string.IsNullOrEmpty(updatedItem.Description))
        {
            this.Description = updatedItem.Description;
        }

        if (updatedItem.Price != 0)
        {
            this.Price = updatedItem.Price;
        }

        if (updatedItem.Quantity != 0)
        {
            this.Quantity = updatedItem.Quantity;
        }
    }
}
