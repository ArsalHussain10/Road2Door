﻿namespace Road2Door.Models.Repository;

using Microsoft.EntityFrameworkCore;
using Road2Door.Models;
using System.Security.Cryptography.Xml;

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
    public List<MenuConsumer> GetNotifications(int consumerId)
    {
        Road2DoorContext road2DoorContext = new Road2DoorContext();
        List<MenuDetail> menus = new List<MenuDetail>();
        List<MenuConsumer> menuConsumers = new List<MenuConsumer>();


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

            foreach (int menueId in menuIds)
            {
                List<MenuDetail> menuDetails = new List<MenuDetail>();

                MenuConsumer singleMenuConsumer = new MenuConsumer();
                menuDetails = road2DoorContext.MenuDetails
                    .Where(m => m.MenueId == menueId).ToList();
                List<Item> itemsOfSingleMenu= new List<Item> ();
                itemsOfSingleMenu = PopulateItemsOfMenu(menueId);
                singleMenuConsumer.MenuDetails = menuDetails;
                singleMenuConsumer.Items= itemsOfSingleMenu;
                menuConsumers.Add(singleMenuConsumer);

                
            }


        }
        return menuConsumers;
    }



    public List<Item> PopulateItemsOfMenu(int menuId)
    {
        Road2DoorContext road2DoorContext = new Road2DoorContext();
        List<int> itemIds = GetItemIdsOfMenu(menuId);
        List<Item> items = new List<Item>();
        foreach (int itemId in itemIds)
        {
            Item singleItem=road2DoorContext.Items.FirstOrDefault(i=> i.ItemId == itemId);
            items.Add(singleItem);
        }
        return items;

    }
    public List<int> GetItemIdsOfMenu(int menuId)
    {
        Road2DoorContext road2DoorContext = new Road2DoorContext();
        List<int> itemIds = road2DoorContext.MenuDetails
            .Where(m => m.MenueId == menuId)
            .Select(m => m.ItemId)
            .ToList();
        return itemIds;
    }
}
