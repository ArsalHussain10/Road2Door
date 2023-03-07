using Microsoft.AspNetCore.Mvc;
using Road2Door.Models;
using Road2Door.Models.Repository;

namespace Road2Door.Controllers
{
    public class RiderController : Controller
    {
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(IFormCollection form)
        {
            string name = form["name"];
            string email = form["email"];
            string password = form["password"];
            string cnic = form["cnic"];
            string phone = form["phoneNumber"];
            IFormFile license = form.Files.FirstOrDefault(x => x.Name == "License");
            IFormFile policeRecord = form.Files.FirstOrDefault(x => x.Name == "CriminalRecord");

            string licenseName = license.FileName;
            string policeRecordName = policeRecord.FileName;

            Rider rider = new Rider {
                Name = name,
                Email = email,
                Password = password,
                Cnic = cnic,
                Phone = phone,
                License = licenseName,
                PoliceRecord = policeRecordName,
                Status = 0
                
            };
            RiderRepository riderRepository= new RiderRepository();
            riderRepository.SignUp(rider);
            
         
            


            // Do something with the form data and return a response
            // For example, you can store the data in a database or send an email

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
            if(riderRepository.CheckAccount(email,password))
            {
                return View("HomePage");
            }
            return View();

        }

    }
}
