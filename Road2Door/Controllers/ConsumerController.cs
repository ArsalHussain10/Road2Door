using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Road2Door.Models;
using Road2Door.Models.Repository;
using Newtonsoft.Json;


namespace Road2Door.Controllers
{
    public class ConsumerController : Controller
    {
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(string name, string email, string password, string phoneNumber)
        {
            ConsumerRepository consumerRepository = new ConsumerRepository();
            if (consumerRepository.CheckIfEmailExist(email) == false)
            {


                Consumer consumer = new Consumer
                {
                    Name = name,
                    Email = email,
                    Password = password,
                    Phone = phoneNumber
                };
                consumerRepository.SignUp(consumer);
                return View("SignIn");
            }
            else
            {
                ViewBag.accountError = "Account with this email already exist";
                return View("SignUp");
            }
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(string email, string password)
        {
            ConsumerRepository consumerRepository = new ConsumerRepository();
            if (consumerRepository.CheckAccount(email, password))
            {
                HttpContext.Response.Cookies.Append("email", email, new Microsoft.AspNetCore.Http.CookieOptions() { SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax, IsEssential = true });
                Consumer consumer = consumerRepository.GetConsumer(email);

                return View("HomePage", consumer);
            }
            return View();

        }
        [HttpGet]
        public IActionResult Logout()
        {
            foreach (var cookie in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Settings()
        {
            if (Request.Cookies["email"]==null)
                return RedirectToAction("Index","Home");
            string userEmail = Request.Cookies["email"];
            ConsumerRepository consumerRepository = new ConsumerRepository();
            Consumer consumer = consumerRepository.GetConsumer(userEmail);

            return View(consumer);
        }

        public IActionResult EditSettings(int id, string name, string email, string password, string contact)
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            Consumer originalConsumer = road2DoorContext.Consumers.Find(id);
            if (originalConsumer != null)
            {
                originalConsumer.Id = id;
                originalConsumer.Name = name;
                originalConsumer.Email = email;
                originalConsumer.Password = password;
                originalConsumer.Phone = contact;

                road2DoorContext.SaveChanges();
            }
            else
            {
                // Handle the case where the item is not found in the database
                throw new Exception($"Item with ID {email} not found.");
            }

            return RedirectToAction("HomePage");

        }


        public IActionResult HomePage()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            string email = Request.Cookies["email"];
            ConsumerRepository consumerRepository = new ConsumerRepository();
            Consumer consumer = consumerRepository.GetConsumer(email);
            return View(consumer);

        }
        [HttpPost]
        public ActionResult UpdateLocation(decimal latitude, decimal longitude)
        {
            // Store the latitude and longitude in your database
            // You can use Entity Framework or any other data access method

            // Return a response if needed
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            string consumerEmail = Request.Cookies["email"];
            ConsumerRepository consumerRepository = new ConsumerRepository();
            Consumer consumer = consumerRepository.GetConsumer(consumerEmail);

            consumerRepository.updateConsumerLocation(consumer.Id, latitude, longitude);
            Console.WriteLine(latitude);
            Console.WriteLine(longitude);
            return Content("Location stored successfully!");
        }

        public IActionResult CheckForNotification()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            // Retrieve the notifications from the database or any other data source
            ConsumerRepository consumerRepository = new ConsumerRepository();
            string email = Request.Cookies["email"];
            Consumer consumer = consumerRepository.GetConsumer(email);
            //List<MenuDetail> menuDetails = consumerRepository.GetNotifications(consumer.Id);
            int count = consumerRepository.GetNotificationsCount(consumer.Id);

            // Return the notifications as JSON
            return new JsonResult(count);
        }

        public IActionResult ShowMenus()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            ConsumerRepository consumerRepository = new ConsumerRepository();
            string email = Request.Cookies["email"];
            Consumer consumer = consumerRepository.GetConsumer(email); ;
            List<MenuConsumer> menuConsumer = consumerRepository.GetNotifications(consumer.Id);

            //foreach(MenuDetail menuDetail in menuDetails)
            //{
            //    Console.WriteLine(menuDetail.Item.Name);
            //}

            return View(menuConsumer);




        }
        [HttpGet]
        public IActionResult PlaceOrder(int menuId)
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            Console.WriteLine(menuId);
            ConsumerRepository consumerRepository = new ConsumerRepository();
            MenuConsumer singleMenuConsumer = consumerRepository.GetSingleMenuConsumer(menuId);
            ViewBag.menuId = menuId;

            return View(singleMenuConsumer);
        }

        

        [HttpPost]
        public IActionResult PlaceOrder(int menuId, Dictionary<int, int> orderQuantities)
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            ConsumerRepository consumerRepository = new ConsumerRepository();
            string email = Request.Cookies["email"];
            Consumer consumer = consumerRepository.GetConsumer(email);
            Order order = new Order();
            order.ConsumerId = consumer.Id;
            order.MenuId = menuId;
            order.Date = DateTime.Now.ToString("yyyy-MM-dd");
            int orderId=consumerRepository.MakeOrder(order);

            if (orderId > 0)
            {



                List<OrderDetail> orderList = new List<OrderDetail>();


                MenuConsumer singleMenuConsumer = consumerRepository.GetSingleMenuConsumer(menuId);
                RiderRepository riderRepository = new RiderRepository();
                int riderId = riderRepository.GetRiderIdFromMenuId(menuId);

                foreach (MenuDetail menuDetail in singleMenuConsumer.MenuDetails)
                {
                    if (orderQuantities.TryGetValue(menuDetail.ItemId, out int quantity))
                    {
                        if (quantity != 0)
                        {


                            OrderDetail singleOrderDetail = new OrderDetail();
                            Item item = consumerRepository.GetItem(menuDetail.ItemId);
                            singleOrderDetail.ItemId = menuDetail.ItemId;
                            singleOrderDetail.Quantity = quantity;
                            singleOrderDetail.OrderId = orderId;
                            //singleOrderDetail.Item = item;
                            orderList.Add(singleOrderDetail);

                            //menuDetail.Quantity -= quantity;
                            //consumerRepository.UpdateMenuDetail(menuDetail);
                        }
                    }
                }


                // Save the updated menu consumer in the database
                //consumerRepository.UpdateMenuConsumer(singleMenuConsumer);
                consumerRepository.AddOrderDetails(orderList);
                OrderNotification orderNotification = new OrderNotification();
                orderNotification.OrderId = orderId;
                orderNotification.RiderId = riderId;
                orderNotification.View = 0;
                consumerRepository.MakeOrderNotification(orderNotification);
               // return Json(new { success = true });
                return RedirectToAction("HomePage");

            }
            else
            {
                return RedirectToAction("HomePage");

               // return Json(new { success = false });
            }
            return RedirectToAction("HomePage");

            // return View("OrderDetails",orderList);
        }


        public IActionResult AddToCart(int itemId, int quantity, int menuId)
        {

            return View("PlaceOrder");
        }

        public IActionResult ChatBox()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            ConsumerRepository consumerRepository = new ConsumerRepository();
            string email = Request.Cookies["email"];
            Consumer consumer=consumerRepository.GetConsumer(email);
            string name=consumer.Name;
            ViewBag.Name = name;
            Console.Write("here in Chat Box");
            return View("ChatBox");
        }

        public IActionResult OrderHistory()
        {
            
               if (Request.Cookies["email"] == null)
                    return RedirectToAction("Index", "Home");
                ConsumerRepository consumerRepository = new ConsumerRepository();
                string email = Request.Cookies["email"];
                Consumer consumer = consumerRepository.GetConsumer(email);
            //    List<Order> orders= consumerRepository.GetOrders(consumer.Id);
            //    RiderRepository riderRepository = new RiderRepository();
            //    List<ConsumerOrder> allOrders = new List<ConsumerOrder>();
            //    foreach(Order order in orders)
            //    {
            //       // Console.WriteLine(order.OrderId);
            //        int riderId = riderRepository.GetRiderIdFromOrderId(order.OrderId);
            //       // Console.WriteLine(riderId);
            //        Rider rider = riderRepository.GetRider(riderId);
            //        //Console.WriteLine(rider.Name);

            //        ConsumerOrder singleOrder = new ConsumerOrder();
            //        singleOrder.Rider = rider;
            //        singleOrder.OrderId = order.OrderId;
            //        singleOrder.OrderDetails = riderRepository.GetOrderDetails(singleOrder.OrderId);
            //        singleOrder.OrderDetails = riderRepository.PopulateOrderItems(singleOrder.OrderDetails);

            //        allOrders.Add(singleOrder);

            //    }
            AdminRepository adminRepository = new AdminRepository();
            List<RiderOrder> orders = adminRepository.GetOrders();
            RiderRepository riderRepository = new RiderRepository();

            List<RiderOrder> filteredOrders = orders.Where(o => o.Consumer.Name == consumer.Name).ToList();
            List<ConsumerOrder> consumerOrders = new List<ConsumerOrder>();
            foreach(RiderOrder f in filteredOrders)
            {
                ConsumerOrder c = new ConsumerOrder();
                c.OrderDetails = f.OrderDetails;
                c.OrderId = f.OrderId;
                Rider r = riderRepository.GetRider(f.RiderId);
                c.Rider = r;
                consumerOrders.Add(c);

            }

            return View(consumerOrders);
        }




        
    }
}
