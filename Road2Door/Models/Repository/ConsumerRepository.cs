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
}
