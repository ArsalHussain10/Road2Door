namespace Road2Door.Models.Repository
{
    public class OrderRepository
    {
        public Order CheckMenuItemExist(int itemId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            Order item = road2DoorContext.Orders.FirstOrDefault(o => o.ItemId == itemId);
            return item;
        }
    }
}
