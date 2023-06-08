namespace Road2Door.Models.Repository;
using Road2Door.Models;

public class ConsumerRepository
{
    public void SignUp(Consumer consumer)
    {
        Road2DoorContext road2Door = new Road2DoorContext();
        road2Door.Consumers.Add(consumer);

        road2Door.SaveChanges();
        ConsumerLocation consumerLocation = new ConsumerLocation();
        consumerLocation.ConsumerId = consumer.Id;
        consumerLocation.Latitude = 0;
        consumerLocation.Longitude = 0;
        road2Door.ConsumerLocations.Add(consumerLocation);
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
    public List<ConsumerLocation> GetAllConsumersLocation()
    {
        decimal targetValue = 0.0000m; // Replace with the desired non-zero value

        Road2DoorContext road2DoorContext = new Road2DoorContext();
        return road2DoorContext.ConsumerLocations
            .Where(cl => cl.Latitude != targetValue)
            .ToList();
    }


    public void updateConsumerLocation(int consumerId, decimal latitude, decimal longitude)
    {
        Road2DoorContext road2DoorContext = new Road2DoorContext();
        ConsumerLocation consumerLocation = road2DoorContext.ConsumerLocations.FirstOrDefault(c => c.ConsumerId == consumerId);
        consumerLocation.Latitude = latitude;
        consumerLocation.Longitude = longitude;
        road2DoorContext.ConsumerLocations.Update(consumerLocation);
        road2DoorContext.SaveChanges();
    }

}
