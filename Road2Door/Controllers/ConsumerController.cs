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
    }
}
