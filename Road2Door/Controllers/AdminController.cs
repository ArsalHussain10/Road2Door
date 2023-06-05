using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc;
using Road2Door.Models;
using Road2Door.Models.Repository;

namespace Road2Door.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult AdminHome()
        {
            return View();
        }

        public IActionResult ShowRiders()
        {
            AdminRepository adminRepository = new AdminRepository();
            List<Rider> riders=adminRepository.GetRiders();

            
            return View(riders);
        }
        public IActionResult ShowConsumers()
        {
            AdminRepository adminRepository = new AdminRepository();
            List<Consumer> consumers=adminRepository.GetConsumers();

            
            return View(consumers);
        }

        [HttpGet]
        public IActionResult ChangeAccountStatusRider(int riderId, int accountStatus)
        {
            AdminRepository adminRepository = new AdminRepository();
            adminRepository.ChangeAccountStatusRider(riderId, accountStatus);
            return RedirectToAction("ShowRiders");

        }
    }
}
