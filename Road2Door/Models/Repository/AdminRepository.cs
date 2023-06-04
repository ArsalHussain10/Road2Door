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
        [HttpGet]
        public List<Consumer> GetConsumers()
        {
            Road2DoorContext road2DoorContext= new Road2DoorContext();
            return road2DoorContext.Consumers.ToList();

        }
    }
}
