﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Road2Door.Models.Repository
{
    public class AdminRepository
    {
        public AdminRepository() { }

        public List<Rider> GetRiders()
        {
            Road2DoorContext road2DoorContext= new Road2DoorContext();
            return road2DoorContext.Riders.Where(r => r.Status != 2).ToList();

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
        public List<Rider> GetRidersRequest()
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();
            return road2DoorContext.Riders.Where(r => r.Status == 2).ToList();

        }

        public void AccountRequest(int riderId, int accountRequest)
        {
            Road2DoorContext road2DoorContext = new Road2DoorContext();

            if (accountRequest==0)
            {
                Rider rider = road2DoorContext.Riders.Find(riderId);
                road2DoorContext.Riders.Remove(rider);
                road2DoorContext.SaveChanges();

            }
            else
            {
                RiderLocation riderLocation = new RiderLocation
                {
                    RiderId = riderId,
                    Latitude = 0,
                    Longitude = 0
                };
                road2DoorContext.RiderLocations.Add(riderLocation);
                road2DoorContext.SaveChanges();
                
                ChangeAccountStatusRider(riderId, 1);
            }
        }

    }
}
