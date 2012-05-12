using System;
using System.Collections.Generic;
using System.Data.Entity;
using Model;

namespace Tests
{
    public class TripContextInitializerForTests : DropCreateDatabaseAlways<Fakes.FakeTravelClubContext>
    {
        protected override void Seed(Fakes.FakeTravelClubContext context)
        {
            new List<Trip>
            {
                new Trip { Id = new Guid("CDC55BE1-4C80-49CC-9026-EEB16EAABB01"), Name = "New York, Hockey 2011" ,DateOfDeparture = DateTime.Now,Description = "",Destination = "",PointOfDeparture = "",TripLengthInDays = 5},
                new Trip { Id = new Guid("CDC55BE1-4C80-49CC-9026-EEB16EAABB02"), Name = "France, Food 2011",DateOfDeparture = DateTime.Now,Description = "",Destination = "",PointOfDeparture = "",TripLengthInDays = 5 },
                new Trip{Id = new Guid("CDC55BE1-4C80-49CC-9026-EEB16EAABB03"), Name = "Turkey, Culture 2012",DateOfDeparture = DateTime.Now,Description = "",Destination = "",PointOfDeparture = "",TripLengthInDays = 5}
            }.ForEach(x => context.Trips.Add(x));

            context.SaveChanges();
        }
    }

}   