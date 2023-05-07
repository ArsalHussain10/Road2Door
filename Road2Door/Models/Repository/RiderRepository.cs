using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Road2Door.Models.Repository
{
    public class RiderRepository
    {
        public RiderRepository() { }

        public void SignUp(Rider rider)
        {
            Road2DoorContext road2Door = new Road2DoorContext();
            road2Door.Riders.Add(rider);
            road2Door.SaveChanges();

        }
        public bool CheckAccount(string email, string password)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            bool hasRider = road2DoorContext.Riders.Any(r => r.Email == email && r.Password == password);
            return hasRider;

        }
        public int CreateItem(Item item)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            road2DoorContext.Items.Add(item);
            road2DoorContext.SaveChanges();
            return item.ItemId;
        }

        public int GetRiderId(string email)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            Rider rider = road2DoorContext.Riders.FirstOrDefault(r => r.Email == email);
            return rider.Id;
        }
        public void StoreInventoryItem(InventoryItem inventoryItem)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            road2DoorContext.InventoryItems.Add(inventoryItem);
            road2DoorContext.SaveChanges();

        }

        public List<int> GetItemIds(int riderId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            List<int> itemIds = road2DoorContext.InventoryItems
                                                   .Where(i => i.RiderId == riderId)
                                                   .Select(i => i.ItemId)
                                                   .ToList();
            return itemIds;
        }

        public List<Item> GetItems(List<int> itemIds)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();

            List<Item> items= new List<Item>();
            foreach (var itemId in itemIds)
            {
                Item item = road2DoorContext.Items.FirstOrDefault(i => i.ItemId == itemId);

                if (item != null)
                {
                    items.Add(item);
                }
            }

            return items;

        }
        public void UpdateItem(int itemId, string description, double price, int quantity)
        {

            Console.WriteLine(itemId);
            using (Road2DoorContext road2DoorContext = new Road2DoorContext())
            {
                // Get the item from the database using the itemId
                Item originalItem = road2DoorContext.Items.Find(itemId);

                if (originalItem != null)
                {
                    // Update only the attributes that are not null or empty
                    originalItem.Description = description ?? originalItem.Description;
                    originalItem.Price = (price > 0) ? price : originalItem.Price;
                    originalItem.Quantity = (quantity > 0) ? quantity : originalItem.Quantity;

                    road2DoorContext.SaveChanges();
                }
                else
                {
                    // Handle the case where the item is not found in the database
                    throw new Exception($"Item with ID {itemId} not found.");
                }
            }
        }
        public void DeleteItem(int itemId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            Item itemToDelete= road2DoorContext.Items.SingleOrDefault(i => i.ItemId == itemId);
           // InventoryItem inventoryItem=road2DoorContext.InventoryItems.SingleOrDefault()

            if (itemToDelete != null)
            {
                road2DoorContext.Items.Remove(itemToDelete);
                road2DoorContext.SaveChanges();
            }
               
        }

    }
}
