using System.Data.Entity;
using Model;

namespace Tests.Fakes
{

    public class FakeTravelClubContext :DbContext, ITravelClubEntitiesContext
    {
        public FakeTravelClubContext()
        {
            this.Trips = new FakeTripSet();
        }

        public IDbSet<Trip> Trips { get; set; }
        public new int SaveChanges()
        {
            return 0;
        }
    }
}