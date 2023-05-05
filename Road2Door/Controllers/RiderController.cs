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
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp([FromForm] Rider r)
        {

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
            rider.Status = 0;

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
            RiderRepository riderRepository = new RiderRepository();
            riderRepository.SignUp(rider);


            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(string email, string password)
        {
            RiderRepository riderRepository = new RiderRepository();
            if (riderRepository.CheckAccount(email, password))
            {
                HttpContext.Response.Cookies.Append("email", email, new Microsoft.AspNetCore.Http.CookieOptions() { SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax, IsEssential = true });

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

            return RedirectToAction("Inventory");

        }
        


    }
}
