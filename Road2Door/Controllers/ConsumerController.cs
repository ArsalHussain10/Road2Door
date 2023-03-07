using Microsoft.AspNetCore.Mvc;

namespace Road2Door.Controllers
{
    public class ConsumerController : Controller
    {
        public IActionResult SignUp()
        {
            return View();
        }
    }
}
