using Microsoft.AspNetCore.Mvc;
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
        public IActionResult SignUp(IFormCollection form)
        {
            string name = form["name"];
            string email = form["email"];
            string password = form["password"];
            string cnic = form["cnic"];
            string phone = form["phoneNumber"];
            List<IFormFile> files = new List<IFormFile>();
            files = Request.Form.Files.ToList();
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
            foreach (var file in files)
            {
                if (i == 0)
                {
                     license = Path.GetFileName(file.FileName);

                    var pathWithFileName = Path.Combine(path1, license);
                    i++;
                    using (FileStream stream = new
                        FileStream(pathWithFileName,
                        FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                else
                {
                    policeRecord = Path.GetFileName(file.FileName);

                    var pathWithFileName = Path.Combine(path2, policeRecord);
                    using (FileStream stream = new
                        FileStream(pathWithFileName,
                        FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
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

        [HttpGet]
        public IActionResult Snapshots()
        {
            return View();
        }

    }
}
