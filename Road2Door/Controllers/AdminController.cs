using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc;
using Road2Door.Models;
using Road2Door.Models.Repository;

namespace Road2Door.Controllers
{
    public class AdminController : Controller
    {
       
        //[HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult AdminHome()
        {

            AdminRepository adminRepository = new AdminRepository();
            List<Consumer> consumers = adminRepository.GetConsumers();
            int consumerCount = consumers.Count();
            ViewBag.consumerCount = consumerCount;
            List<Rider> riders = adminRepository.GetRiders();
            int riderCount = riders.Count();
            ViewBag.riderCount = riderCount;
            List<Rider> pendingReq = adminRepository.GetRidersRequest();
            int pendingRequests = pendingReq.Count();
            ViewBag.pendingRequests = pendingRequests;
            int acceptedReq = adminRepository.RidersAcceptedRequest();
            ViewBag.acceptedRequests = acceptedReq;
            int deactivatedRiders = adminRepository.DeactivatedAccountsRider();
            ViewBag.deactivatedRiders = deactivatedRiders;

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

        [HttpGet]

        public IActionResult RiderAccountRequest()
        {
            AdminRepository adminRepository = new AdminRepository();
            List<Rider> riders=adminRepository.GetRidersRequest();
            return View(riders);
        }
        public IActionResult AccountRequest(int riderId, int accountRequest)
        {
            AdminRepository adminRepository = new AdminRepository();
            adminRepository.AccountRequest(riderId,accountRequest);
            return RedirectToAction("RiderAccountRequest");
        }
        public IActionResult ViewOrders()
        {
            AdminRepository adminRepository = new AdminRepository();
            List<OrderNotification> approvedOrders = adminRepository.GetApprovedOrders();
            RiderRepository riderRepository = new RiderRepository();
            List<RiderOrder> orders = new List<RiderOrder>();
            foreach (OrderNotification singleApprovedOrder in approvedOrders)
            {
                RiderOrder riderOrder = new RiderOrder();
                riderOrder.RiderId = singleApprovedOrder.RiderId;
                riderOrder.OrderId = singleApprovedOrder.OrderId;
                riderOrder.OrderDetails = riderRepository.GetOrderDetails(riderOrder.OrderId);
                riderOrder.OrderDetails = riderRepository.PopulateOrderItems(riderOrder.OrderDetails);
                riderOrder.Consumer = riderRepository.GetConsumerThroughOrderId(riderOrder.OrderId);
                orders.Add(riderOrder);
            }
            orders.Reverse();
            return View(orders);
        }
        public IActionResult AdminProfile()
        {
            return View();
        }
    }
}
