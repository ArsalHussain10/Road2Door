using Microsoft.AspNetCore.Mvc;

namespace Road2Door.Models.Repository
{
    public class AdminRepository
    {
        public AdminRepository() { }

        public List<Rider> GetRiders()
        {
            Road2DoorContext road2DoorContext= new Road2DoorContext();
            return road2DoorContext.Riders.ToList();

        }
        public List<Consumer> GetConsumers()
        {
            Road2DoorContext road2DoorContext= new Road2DoorContext();
            return road2DoorContext.Consumers.ToList();

        }

        public void ChangeAccountStatusRider(int riderId, int accountStatus)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            Rider rider = road2DoorContext.Riders.FirstOrDefault(r => r.Id == riderId);
            if (rider != null)
            {
                rider.Status = accountStatus;
                road2DoorContext.SaveChanges();
                // Account status updated successfully
            }
        }

    }
}
