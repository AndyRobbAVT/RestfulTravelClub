using System.Data.Entity;

namespace Model
{
    public interface ITravelClubEntitiesContext
    {
        IDbSet<Trip> Trips { get; set; }
        int SaveChanges();
    }

}
