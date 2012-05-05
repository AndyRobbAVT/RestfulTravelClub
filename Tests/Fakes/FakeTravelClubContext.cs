using System.Data.Entity;
using Model;

namespace Tests.Fakes
{

    public class FakeTravelClubContext : ITravelClubEntitiesContext
    {
        public FakeTravelClubContext()
        {
            this.Trips = new FakeTripSet();
        }

        public IDbSet<Trip> Trips { get; set; }
        public int SaveChanges()
        {
            return 0;
        }
    }
}