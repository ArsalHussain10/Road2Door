namespace Road2Door.Models.Repository
{
    public class RiderRepository
    {
        public RiderRepository() { }

        public void SignUp(Rider rider)
        {
            Road2DoorContext road2Door = new Road2DoorContext();
            road2Door.Riders.Add(rider);
            road2Door.SaveChanges();

        }
        public bool CheckAccount(string email, string password)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            bool hasRider = road2DoorContext.Riders.Any(r => r.Email == email && r.Password == password);
            return hasRider;

        }
    }
}
