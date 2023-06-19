using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Road2Door.Models;
using Road2Door.Models.Repository;
using System.IO;

namespace Road2Door.Controllers
{
    public class RiderController : Controller
    {
        private readonly IWebHostEnvironment Environment;
        public RiderController(IWebHostEnvironment
           environment)
        {

            Environment = environment;
        }
        [HttpGet]
        public IActionResult Logout()
        {
            DeleteNotifications();
            foreach (var cookie in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ShowSignUpPage()
        {
            return View("SignUp");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp([FromForm] Rider r)
        {
            RiderRepository riderRepository= new RiderRepository();
            if (riderRepository.CheckIfEmailExist(r.Email) == false)
            {

                Console.WriteLine("no account does not exist");


                var name = r.Name;
                var email = r.Email;
                var password = r.Password;
                var cnic = r.Cnic;
                var phone = r.Phone;
                var file1 = Request.Form.Files.GetFile("License");
                var file2 = Request.Form.Files.GetFile("CriminalRecord");


                //IFormFile license = form.Files.FirstOrDefault(x => x.Name == "License");
                //IFormFile policeRecord = form.Files.FirstOrDefault(x => x.Name == "CriminalRecord");

                string wwwPath = this.Environment.WebRootPath;
                string subPath1 = "uploads\\Licnese\\";
                string subPath2 = "uploads\\CriminalRecord\\";
                string path1 = Path.Combine(wwwPath, subPath1);
                string path2 = Path.Combine(wwwPath, subPath2);


                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }
                int i = 0;
                var license = "";
                var policeRecord = "";
                license = Path.GetFileName(file1.FileName);

                var pathWithFileName = Path.Combine(path1, license);
                i++;
                using (FileStream stream = new
                    FileStream(pathWithFileName,
                    FileMode.Create))
                {
                    file1.CopyTo(stream);
                }



                policeRecord = Path.GetFileName(file2.FileName);

                var pathWithFileName2 = Path.Combine(path2, policeRecord);
                using (FileStream stream = new
                    FileStream(pathWithFileName2,
                    FileMode.Create))
                {
                    file2.CopyTo(stream);
                }


                Rider rider = new Rider();
                rider.Name = name;
                rider.Email = email;
                rider.Password = password;
                rider.Cnic = cnic;
                rider.Phone = phone;
                rider.License = license;
                rider.PoliceRecord = policeRecord;
                rider.Status = 2;   //request will be send to admin

                /*   Rider rider1 = new Rider {
                       Name = name,
                       Email = email,
                       Password = password,
                       Cnic = cnic,
                       Phone = phone,
                       License = license,
                       PoliceRecord = policeRecord,
                       Status = 0

                   };

                */
                riderRepository.SignUp(rider);


                return View("SignIn");
            }
            else
            {
                Console.WriteLine("oops account already exist");
                ViewBag.accountError = "account with this email already exist";
                return View("SignUp");
            }
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult HomePage()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            string riderEmail = Request.Cookies["email"];
            RiderRepository riderRepository = new RiderRepository();
            string riderName = riderRepository.GetRiderName(riderEmail);
            ViewBag.riderName = riderName;
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(string email, string password)
        {
            RiderRepository riderRepository = new RiderRepository();
            if (riderRepository.CheckAccount(email, password))
            {
                int accountStatus = riderRepository.CheckAccountStatus(email, password);
                if (accountStatus == 2)
                {
                    ViewBag.message = "your account has yet not been approved by the admin";
                    return View();
                }
                else if (accountStatus == 0)
                {
                    ViewBag.message = "your account has been deactivated by the admin";
                    return View();

                }

                HttpContext.Response.Cookies.Append("email", email, new Microsoft.AspNetCore.Http.CookieOptions() { SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax, IsEssential = true });
                string riderEmail = Request.Cookies["email"];

                string riderName = riderRepository.GetRiderName(email);
                ViewBag.riderName = riderName;
                return View("HomePage");
            }

            return View();

        }


        [HttpGet]
        public IActionResult Snapshots()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Inventory()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            string riderEmail = Request.Cookies["email"];
            RiderRepository riderRepository = new RiderRepository();
            int riderId = riderRepository.GetRiderId(riderEmail);
            List<int> itemIds = riderRepository.GetItemIds(riderId);
            List<Item> items = riderRepository.GetItems(itemIds);
            return View(items);
        }

        [HttpPost]
        public IActionResult Inventory(string name, string description, string quantity, string price)
        {

            Item item = new Item
            {
                Description = description,
                Name = name,
                Quantity = int.Parse(quantity),
                Price = Double.Parse(price),

            };

            RiderRepository riderRepository = new RiderRepository();
            int itemId = riderRepository.CreateItem(item);

            string riderEmail = Request.Cookies["email"];
            int riderId = riderRepository.GetRiderId(riderEmail);

            InventoryItem inventoryItem = new InventoryItem
            {
                RiderId = riderId,
                ItemId = itemId,
            };

            riderRepository.StoreInventoryItem(inventoryItem);



            return RedirectToAction("Inventory");

        }

        public IActionResult EditItem(int itemId, string description, double price, int quantity)
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            Road2DoorContext road2DoorContext = new Road2DoorContext();
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

            return RedirectToAction("Inventory");

        }

        public IActionResult DeleteItem(int itemId)
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            RiderRepository riderRepository = new RiderRepository();
            riderRepository.DeleteItem(itemId);
            return RedirectToAction("Inventory");
        }

        public IActionResult Menu()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            RiderRepository riderRepository = new RiderRepository();
            string riderEmail = Request.Cookies["email"];
            int riderId = riderRepository.GetRiderId(riderEmail);
            int menuId = riderRepository.GetMenuId(riderId);
            List<int> itemIds = riderRepository.GetItemIds(riderId);
            List<Item> items = riderRepository.GetItems(itemIds);
            List<MenuDetail> menuitem = riderRepository.GetMenuItem(menuId);

            ViewBag.menuId = menuId;
            ViewBag.items = items;
            ViewBag.menuItems = menuitem;

            if (menuId != -1)
            {
                return View("MenuPage");
            }
            else
            {
                string creationDate = DateTime.Now.ToString("yyyy-MM-dd");
                MenueMaster record = new MenueMaster
                {
                    RiderId = riderId,
                    CreationDate = creationDate,

                };
                riderRepository.AddToMenuMaster(record);
                return RedirectToAction("Menu");

            }

        }
        public IActionResult AddToMenu(int itemId, int quantity)
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");

            RiderRepository riderRepository = new RiderRepository();
            string riderEmail = Request.Cookies["email"];
            int riderId = riderRepository.GetRiderId(riderEmail);
            int menuId = riderRepository.GetMenuId(riderId);
            Item itemFound = riderRepository.GetItem(itemId);
            if (itemFound != null)
            {
                ViewBag.maxQuantity = itemFound.Quantity;
                int newQuantity = itemFound.Quantity - quantity;
                riderRepository.updateQuantity(itemId, newQuantity);
            }
            List<int> itemIds = riderRepository.GetItemIds(riderId);
            List<Item> items = riderRepository.GetItems(itemIds);
            ViewBag.items = items;
            MenuDetail mItem = riderRepository.CheckMenuItemExist(itemId);
            if (mItem != null)
            {
                int updateQuantity = mItem.Quantity + quantity;
                riderRepository.updateQuantityMenuItem(itemId, updateQuantity);

            }
            else
            {
                MenuDetail menuItem = new MenuDetail
                {
                    MenueId = menuId,
                    ItemId = itemId,
                    Quantity = quantity,
                };
                riderRepository.AddToMenuDetails(menuItem);
            }
            List<MenuDetail> menuitem = riderRepository.GetMenuItem(menuId);
            ViewBag.menuItems = menuitem;
            ViewBag.menuId = menuId;

            return View("MenuPage");
        }


        public IActionResult DeleteItemFromMenu(int itemId, int quantity)
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            RiderRepository riderRepository = new RiderRepository();

            Item itemFound = riderRepository.GetItem(itemId);
            if (itemFound != null)
            {
                int newQuantity = itemFound.Quantity + quantity;
                riderRepository.addQuantitytoInventoryOnDelete(itemId, newQuantity);
            }
            riderRepository.DeleteItemFromMenu(itemId);

            return RedirectToAction("Menu");
        }
        [HttpPost]
        public IActionResult UpdateMenuItemQuantity(int itemId, int menuItemQuantity, int updatedQuantity)
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            RiderRepository riderRepository = new RiderRepository();
            string riderEmail = Request.Cookies["email"];
            int riderId = riderRepository.GetRiderId(riderEmail);
            int menuId = riderRepository.GetMenuId(riderId);
            List<int> itemIds = riderRepository.GetItemIds(riderId);
            List<Item> items = riderRepository.GetItems(itemIds);
            List<MenuDetail> menuitem = riderRepository.GetMenuItem(menuId);

            ViewBag.menuId = menuId;
            ViewBag.items = items;
            ViewBag.menuItems = menuitem;
            if (updatedQuantity > menuItemQuantity) //25>32
            {
                int newQuantity1 = 0, newQuantity2 = 0;
                int decrementQuantity = updatedQuantity - menuItemQuantity; //18-0

                Item itemFound = riderRepository.GetItem(itemId); //2
                if (itemFound != null && itemFound.Quantity != 0)
                {
                    newQuantity1 = itemFound.Quantity - decrementQuantity;//
                    if (newQuantity1 > 0)
                    {
                        riderRepository.updateQuantity(itemId, newQuantity1);
                    }
                    else
                    {
                        newQuantity1 = 0;
                        riderRepository.updateQuantity(itemId, newQuantity1);

                    }
                }
                MenuDetail mItem = riderRepository.CheckMenuItemExist(itemId);
                if (mItem != null)
                {
                    newQuantity2 = mItem.Quantity + decrementQuantity;
                    riderRepository.updateQuantityMenuItem(itemId, newQuantity2);

                }
                var response = new
                {
                    updatedInventoryQuantity = newQuantity1,
                    updatedMenuQuantity = newQuantity2,
                    menu = menuitem // Include the updated menu in the response

                };

                return Json(response);
                /* return RedirectToAction("Menu");*/
            }
            else if (updatedQuantity < menuItemQuantity) //25<32
            {
                int newQuantity1 = 0, newQuantity2 = 0;
                int Updatequantity = menuItemQuantity - updatedQuantity; //32-25= 7
                Item itemFound = riderRepository.GetItem(itemId);
                if (itemFound != null)
                {
                    newQuantity1 = itemFound.Quantity + Updatequantity; //0+7=7
                    riderRepository.updateQuantity(itemId, newQuantity1);
                }
                MenuDetail mItem = riderRepository.CheckMenuItemExist(itemId); //32
                if (mItem != null)
                {
                    newQuantity2 = mItem.Quantity - Updatequantity; //32-7 =25
                    if (newQuantity2 > 0)
                    {
                        riderRepository.updateQuantityMenuItem(itemId, newQuantity2);
                    }
                    else
                    {
                        newQuantity2 = 0;
                        riderRepository.updateQuantityMenuItem(itemId, newQuantity2);
                    }

                }
                var response = new
                {
                    updatedInventoryQuantity = newQuantity1, //7
                    updatedMenuQuantity = newQuantity2, //25
                    menu = menuitem // Include the updated menu in the response

                };

                return Json(response);
                /*                return RedirectToAction("Menu");
                */
            }
            return RedirectToAction("Menu");
        }
        //[HttpPost]
        public ActionResult UpdateLocation(decimal latitude, decimal longitude)
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            // Store the latitude and longitude in your database
            // You can use Entity Framework or any other data access method

            // Return a response if needed
            string riderEmail = Request.Cookies["email"];
            RiderRepository riderRepository = new RiderRepository();
            int riderId = riderRepository.GetRiderId(riderEmail);

            riderRepository.updateRiderLocation(riderId, latitude, longitude);
            Console.WriteLine(latitude);
            Console.WriteLine(longitude);
            return Content("Location stored successfully!");
        }
        [HttpPost]
        public ActionResult SendMenu()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            string riderEmail = Request.Cookies["email"];
            RiderRepository riderRepository = new RiderRepository();
            int riderId = riderRepository.GetRiderId(riderEmail);
            riderRepository.sendMenu(riderId);
            Console.WriteLine("inside send menu function");

            return Content("hehe");
        }
        [HttpPost]
        public ActionResult NotificationCleanup()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            Console.WriteLine("inside cleanup");
            DateTime threshold = DateTime.Now.AddMinutes(-1); //jis notification ko 1 min se zada hogya delete that.

            using (var road2DoorContext = new Road2DoorContext())
            {
                var expiredNotifications = road2DoorContext.Notifications
                    .Where(n => n.InsertionTime < threshold)
                    .ToList();
                Console.WriteLine(expiredNotifications.Count);
                if (expiredNotifications.Count > 0)
                {
                    road2DoorContext.Notifications.RemoveRange(expiredNotifications);
                    road2DoorContext.SaveChanges();
                    Console.WriteLine("A notification has been deleted");
                    // Only a single row has been returned
                    // Perform your desired actions here
                }
                
            }

            return Content("hehe");

        }

        // if rider want's his menu to be hidden, then delete all the notification of riders menue which is sne dto the consumers
        public IActionResult DeleteNotifications()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            string riderEmail = Request.Cookies["email"];
            RiderRepository riderRepository = new RiderRepository();
            int riderId = riderRepository.GetRiderId(riderEmail);
            riderRepository.DeleteNotifications(riderId);
            return Content("notifications deleted");

        }

        public IActionResult CheckForNotification()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            // Retrieve the notifications from the database or any other data source
            RiderRepository riderRepository = new RiderRepository();
            string email = Request.Cookies["email"];
            int riderId = riderRepository.GetRiderId(email);
            //List<MenuDetail> menuDetails = consumerRepository.GetNotifications(consumer.Id);
            int count = riderRepository.GetOrderNotificationsLength(riderId);

            // Return the notifications as JSON
            return new JsonResult(count);
        }

        public IActionResult ViewOrders()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            RiderRepository riderRepository = new RiderRepository();
            string email = Request.Cookies["email"];
            int riderId = riderRepository.GetRiderId(email);
            List<RiderOrder> riderOrders=riderRepository.GetOrders(riderId);
            riderOrders.Reverse();

            return View(riderOrders);


        }
        public IActionResult ChatBox()
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");

            RiderRepository riderRepository = new RiderRepository();
            string riderEmail = Request.Cookies["email"];
            string riderName=riderRepository.GetRiderName(riderEmail);
            Console.Write("here in Chat Box");
            ViewBag.Name=riderName;
            return View();
        }

        [HttpGet]
        public IActionResult RejectOrders(int orderId)
        {
            if (Request.Cookies["email"] == null)
                return RedirectToAction("Index", "Home");
            RiderRepository riderRepository = new RiderRepository();
            string email = Request.Cookies["email"];

            List<OrderDetail> acceptedOrder = riderRepository.GetOrderDetails(orderId);

            ConsumerRepository consumerRepository = new ConsumerRepository();

            foreach (OrderDetail orderDetail in acceptedOrder)
            {

                MenuDetail menuDetail = riderRepository.GetMenuDetailFromItemId(orderDetail.ItemId);
                menuDetail.Quantity = menuDetail.Quantity - orderDetail.Quantity;
                consumerRepository.UpdateMenuDetail(menuDetail);


            }



            int riderId = riderRepository.GetRiderId(email);
            riderRepository.RejectOrders(riderId, orderId);


            return RedirectToAction("HomePage");
        }


    }
}