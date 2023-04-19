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
    }
}
