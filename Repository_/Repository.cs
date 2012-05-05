using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Repository
{
    public class Repository : IRepository
    {
        private static Dictionary<Guid, Trip> trips = InitializeTrips();

        private static Dictionary<Guid, Trip> InitializeTrips()
        {
            Guid trip1Id = new Guid("CDC55BE1-4C80-49CC-9026-EEB16EAABB01");
            Guid trip2Id = new Guid("CDC55BE1-4C80-49CC-9026-EEB16EAABB02");
            Guid trip3Id = new Guid("CDC55BE1-4C80-49CC-9026-EEB16EAABB03");
            trips = new Dictionary<Guid, Trip>()
                                            {
                                                { trip1Id, new Trip { Id = trip1Id, Name = "New York, Hockey 2011" } },
                                                { trip2Id, new Trip { Id = trip2Id, Name = "France, Food 2011" } },
                                                {trip3Id, new Trip{Id = trip3Id, Name = "Turkey, Culture 2012"}}
                                            };
            return trips;
        }

        public IQueryable<Trip> GetAll()
        {
            return trips.Values.OfType<Trip>().AsQueryable<Trip>();
        }

        public Trip GetById(Guid id)
        {
            return trips.Values.SingleOrDefault(x => x.Id == id);
        }

        public Trip Add(Trip trip)
        {
            trips.Add(trip.Id, trip);
            return trip;
        }

        public void Delete(Guid id)
        {
            Trip TripToDelete = trips.Values.SingleOrDefault(x => x.Id == id);
            if (TripToDelete != null)
                trips.Remove(id);

        }

        public void Update(Trip trip)
        {
            Trip tripToUpdate = trips.Values.SingleOrDefault(x => x.Id == trip.Id);
            if (tripToUpdate != null)
                tripToUpdate = trip;
        }
    }
}