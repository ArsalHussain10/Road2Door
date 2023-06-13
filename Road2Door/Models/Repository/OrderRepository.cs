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
        public void AddToOrder(Order orderItem)
        {

            Road2DoorContext road2DoorContext = new Road2DoorContext();
            road2DoorContext.Orders.Add(orderItem);
            road2DoorContext.SaveChanges();

        }
        public List<Order> GetOrderItem(int menuId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();

            List<Order> items = road2DoorContext.Orders.Where(mItem => mItem.MenuId == menuId).Select(i => i).ToList();
            return items;

        }
    }
}
