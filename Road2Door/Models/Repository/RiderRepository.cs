using Microsoft.Ajax.Utilities;
using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public bool CheckIfEmailExist(string email)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            bool hasRider = road2DoorContext.Riders.Any(r => r.Email == email);
            return hasRider;
        }
        public bool CheckAccount(string email, string password)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            bool hasRider = road2DoorContext.Riders.Any(r => r.Email == email && r.Password == password);
            return hasRider;

        }
        public int CheckAccountStatus(string email, string password)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            Rider rider = road2DoorContext.Riders.FirstOrDefault(r => r.Email == email && r.Password == password);

            int riderStatus = (int)rider.Status;
            return riderStatus;

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
        public string GetRiderName(string email)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            Rider rider = road2DoorContext.Riders.FirstOrDefault(r => r.Email == email);
            return rider.Name;
        }

        public int GetRiderIdFromMenuId(int menuId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            MenueMaster riderId = road2DoorContext.MenueMasters.FirstOrDefault(r => r.MenueId == menuId);
            return riderId.RiderId;
        }

        public RiderLocation GetRiderLocation(int riderId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            return road2DoorContext.RiderLocations.FirstOrDefault(r => r.RiderId == riderId);
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
        public int GetMenuId(int id)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            MenueMaster menuId = road2DoorContext.MenueMasters.FirstOrDefault(r => r.RiderId == id);
            if (menuId == null)
            {
                return -1;
            }
            else
            {
                return menuId.MenueId;
            }
        }
        public Item GetItem(int itemId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            Item item = road2DoorContext.Items.FirstOrDefault(i => i.ItemId == itemId);
            return item;
        }
        
        public void updateQuantity(int itemId,int newQuantity)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            Item item = road2DoorContext.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null) {
                item.Quantity = newQuantity;
                road2DoorContext.SaveChanges();
            }

        }
        public MenuDetail CheckMenuItemExist(int itemId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            MenuDetail item = road2DoorContext.MenuDetails.FirstOrDefault(i => i.ItemId == itemId);
            return item;
        }

        public void updateQuantityMenuItem(int itemId, int newQuantity)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            MenuDetail item = road2DoorContext.MenuDetails.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null)
            {
                item.Quantity = newQuantity;
                road2DoorContext.SaveChanges();
            }
        }

        public void AddToMenuMaster(MenueMaster menuRecord)
        {

            Road2DoorContext road2DoorContext = new Road2DoorContext();
            road2DoorContext.MenueMasters.Add(menuRecord);
            road2DoorContext.SaveChanges();

        }
        public void AddToMenuDetails(MenuDetail menuItem)
        {

            Road2DoorContext road2DoorContext = new Road2DoorContext();
            road2DoorContext.MenuDetails.Add(menuItem);
            road2DoorContext.SaveChanges();

        }
        public List<MenuDetail> GetMenuItem(int menuId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();

            List<MenuDetail> items = road2DoorContext.MenuDetails.Where(mItem=>mItem.MenueId==menuId).Select(i=>i).ToList();
          
            return items;

        }

        public void DeleteItemFromMenu(int itemId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            MenuDetail itemToDelete = road2DoorContext.MenuDetails.SingleOrDefault(i => i.ItemId == itemId);

            if (itemToDelete != null)
            {
                road2DoorContext.MenuDetails.Remove(itemToDelete);
                road2DoorContext.SaveChanges();
            }

        }

        public void addQuantitytoInventoryOnDelete(int itemId, int quantity)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            Item item = road2DoorContext.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null)
            {
                item.Quantity = quantity;
                road2DoorContext.SaveChanges();
            }
        }

        public void updateRiderLocation(int riderId, decimal latitude, decimal longitude)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            RiderLocation riderLocation=road2DoorContext.RiderLocations.FirstOrDefault(r => r.RiderId == riderId);
            riderLocation.Latitude = latitude;
            riderLocation.Longitude = longitude;
            road2DoorContext.RiderLocations.Update(riderLocation);
            road2DoorContext.SaveChanges();
        }

        public void sendMenu(int riderId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            ConsumerRepository consumerRepository = new ConsumerRepository();
            List<ConsumerLocation> consumerLocations = consumerRepository.GetAllConsumersLocation();
            RiderLocation riderLocation = GetRiderLocation(riderId);

            foreach( ConsumerLocation consumerLocation in consumerLocations )
            {
                double distance = CalculateDistance(consumerLocation, riderLocation);
                if(distance<5000)  //5 km
                {
                    if(!CheckIfNotificationAlreadySend(consumerLocation.ConsumerId, riderId))
                    {
                        Notification notification = new Notification()
                        {
                            RiderId = riderId,
                            ConsumerId=consumerLocation.ConsumerId,

                        };
                        road2DoorContext.Notifications.Add(notification);
                        road2DoorContext.SaveChanges();
                        // send notification to consumer
                    }

                }
            }

        }
        

        public double CalculateDistance(ConsumerLocation consumerLocation, RiderLocation riderLocation)
        {
            const double EarthRadius = 6371;

            double dLat = ToRadians( (double) consumerLocation.Latitude - (double)riderLocation.Latitude);
            double dLon = ToRadians((double)consumerLocation.Longitude - (double)riderLocation.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians((double)consumerLocation.Latitude)) * Math.Cos(ToRadians((double)riderLocation.Latitude)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = EarthRadius * c * 1000; // Convert distance to meters

            return distance;


        }
        private static double ToRadians(double degree)
        {
            return degree * (Math.PI / 180);
        }

        public bool CheckIfNotificationAlreadySend(int consumerId, int riderId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            bool rowExists = road2DoorContext.Notifications.Any(n => n.RiderId == riderId && n.ConsumerId == consumerId);
            return rowExists;
        }

        public void DeleteNotifications(int riderId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            var notificationsToDelete = road2DoorContext.Notifications
                .Where(n => n.RiderId == riderId)
                .ToList();

            // Delete the notifications
            road2DoorContext.Notifications.RemoveRange(notificationsToDelete);
            road2DoorContext.SaveChanges();
        }

        public int GetOrderNotificationsLength(int riderId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            List<OrderNotification> orderNotifications = road2DoorContext.OrderNotifications.Where(o => o.RiderId == riderId && o.View == 0).ToList();
            return orderNotifications.Count;
        }
        public List<RiderOrder> GetOrders(int riderId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
           List<OrderNotification> orders=  road2DoorContext.OrderNotifications.Where(o => o.RiderId == riderId && o.View == 0).ToList();
            List<RiderOrder> riderOrders = new List<RiderOrder>();
            ConsumerRepository consumerRepository = new ConsumerRepository();
            foreach(OrderNotification order in orders)
            {
                RiderOrder singleOrder = new RiderOrder();
                singleOrder.OrderId=order.OrderId;
                singleOrder.RiderId = riderId;
                List<OrderDetail> orderDetails = GetOrderDetails(singleOrder.OrderId);
                List<OrderDetail> completeOrderDetails = PopulateOrderItems(orderDetails);
                singleOrder.OrderDetails = completeOrderDetails;
                Consumer consumer = GetConsumerThroughOrderId(singleOrder.OrderId);
                singleOrder.Consumer = consumer;
                riderOrders.Add(singleOrder);

            }
            return riderOrders;
        }
        public Consumer GetConsumerThroughOrderId(int orderId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            Order order = road2DoorContext.Orders.FirstOrDefault(o => o.OrderId == orderId);
            int consumerId = order.ConsumerId;
            Consumer consumer = road2DoorContext.Consumers.FirstOrDefault(c => c.Id == consumerId);
            return consumer;
        }
        
        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            return road2DoorContext.OrderDetails.Where(o => o.OrderId == orderId).ToList();
        }

        public List<OrderDetail> PopulateOrderItems(List<OrderDetail> orderDetails)
        {
            Road2DoorContext road2DoorContext= new Road2DoorContext();
            List<OrderDetail> completeOrderDetails = new List<OrderDetail>();
            foreach(OrderDetail o in orderDetails)
            {
                
                o.Item = GetItem(o.ItemId);
                
                completeOrderDetails.Add(o);

            }
            return completeOrderDetails;

        }
        public void RejectOrders(int riderId, int orderId)
        {
            using (var road2DoorContext = new Road2DoorContext())
            {
                List<OrderNotification> notificationsToUpdate = road2DoorContext.OrderNotifications
                    .Where(notification => notification.RiderId == riderId && notification.OrderId != orderId && notification.View==0 )
                    .ToList();

                foreach (OrderNotification notification in notificationsToUpdate)
                {
                    notification.View = 1;
                }

                var orderNotification = road2DoorContext.OrderNotifications
                    .FirstOrDefault(notification => notification.RiderId == riderId && notification.OrderId == orderId);

                if (orderNotification != null)
                {
                    orderNotification.View = 2;
                }

                road2DoorContext.SaveChanges();
            }
        }

    }

}
