﻿namespace Road2Door.Models.Repository;

using Microsoft.EntityFrameworkCore;
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

    public int GetNotificationsCount(int consumerId)
    {
        Road2DoorContext road2DoorContext = new Road2DoorContext();
        List<MenuDetail> menus = new List<MenuDetail>();

        List<int> riderIds = road2DoorContext.Notifications
    .Where(n => n.ConsumerId == consumerId)
    .Select(n => n.RiderId)
    .ToList();
        return riderIds.Count;

    }
    public List<MenuDetail> GetNotifications(int consumerId)
    {
        Road2DoorContext road2DoorContext = new Road2DoorContext();
        List<MenuDetail> menus = new List<MenuDetail>();

        List<int> riderIds = road2DoorContext.Notifications
    .Where(n => n.ConsumerId == consumerId)
    .Select(n => n.RiderId)
    .ToList();
        if (riderIds.Count() > 0)
        {
            // getting menuids of the riders

            List<int> menuIds = new List<int>();

            foreach (int riderId in riderIds)
            {
                int menueId = road2DoorContext.MenueMasters
                    .Where(rm => rm.RiderId == riderId)
                    .Select(rm => rm.MenueId).FirstOrDefault();

                menuIds.Add(menueId);
            }
            //getting complete menues
            //foreach (int menueId in menuIds)
            //{
            //    MenuDetail menuDetail = road2DoorContext.MenuDetails
            //        .Include(m => m.Item)
            //        .FirstOrDefault(m => m.MenueId == menueId);
            //    menus.Add(menuDetail);

            //    //            MenuDetail menuDetail = road2DoorContext.MenuDetails
            //    //.FirstOrDefault(m => m.MenueId == menueId);

            //    //            if (menuDetail != null)
            //    //            {
            //    //                road2DoorContext.Entry(menuDetail)
            //    //                    .Collection(m => m.Items)
            //    //                    .Load();

            //    //                menus.Add(menuDetail);
            //    //            }


            //}
            foreach (int menueId in menuIds)
            {
                MenuDetail menuDetail = road2DoorContext.MenuDetails
                    .Include(m => m.Item)
                    .Where(m => m.MenueId == menueId)
                    .FirstOrDefault();

                if (menuDetail != null)
                {
                    menus.Add(menuDetail);
                }
            }
            Console.WriteLine(menuIds.ToString());


        }
        return menus;
    }
    //public List<MenuDetail> GetNotifications(int consumerId)
    //{
    //    using (Road2DoorContext road2DoorContext = new Road2DoorContext())
    //    {
    //        List<MenuDetail> menus = road2DoorContext.MenuDetails
    //            .Include(m => m.Item)
    //            .Where(m => road2DoorContext.Notifications.Any(n => n.ConsumerId == consumerId && n.RiderId == m.Menue.MenueId))
    //            .ToList();

    //        return menus;
    //    }
    //}



}
