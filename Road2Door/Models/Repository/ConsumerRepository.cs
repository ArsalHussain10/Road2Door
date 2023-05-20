namespace Road2Door.Models.Repository;
using Road2Door.Models;

public class ConsumerRepository
{
    public void SignUp(Consumer consumer)
    {
        Road2DoorContext road2Door = new Road2DoorContext();
        road2Door.Consumers.Add(consumer);
        road2Door.SaveChanges();

    }
    public bool CheckAccount(string email, string password)
    {
        Road2DoorContext road2DoorContext = new Road2DoorContext();
        bool hasConsumer = road2DoorContext.Consumers.Any(c => c.Email == email && c.Password == password);
        return hasConsumer;

    }

    public Consumer GetConsumer(string email)
    {
        Road2DoorContext road2DoorContext = new Road2DoorContext();
       return road2DoorContext.Consumers.FirstOrDefault(c => c.Email == email);
    }


}
