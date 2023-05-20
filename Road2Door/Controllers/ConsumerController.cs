using Microsoft.AspNetCore.Mvc;
using Road2Door.Models;
using Road2Door.Models.Repository;

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

            Consumer consumer = new Consumer
            {
                Name = name,
                Email = email,
                Password = password,
                Phone = phoneNumber
            };
            ConsumerRepository consumerRepository = new ConsumerRepository();
            consumerRepository.SignUp(consumer);
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
            ConsumerRepository consumerRepository = new ConsumerRepository();
            if (consumerRepository.CheckAccount(email, password))
            {
                HttpContext.Response.Cookies.Append("email", email, new Microsoft.AspNetCore.Http.CookieOptions() { SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax, IsEssential = true });

                return View("HomePage");
            }
            return View();

        }

        public IActionResult Settings()
        {
            string userEmail = Request.Cookies["email"];
            ConsumerRepository consumerRepository = new ConsumerRepository();
            Consumer consumer = consumerRepository.GetConsumer(userEmail);

            return View(consumer);
        }

        public IActionResult EditSettings(int id, string name, string email, string password, string contact)
        {
            Road2DoorContext road2DoorContext= new Road2DoorContext();
            Consumer originalConsumer = road2DoorContext.Consumers.Find(id);
            if(originalConsumer != null)
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
            return View();

        }


    }
}
