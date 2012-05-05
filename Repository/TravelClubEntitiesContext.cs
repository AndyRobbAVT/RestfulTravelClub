using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Model;


namespace Repository
{
    public class TravelClubEntitiesContext :DbContext, ITravelClubEntitiesContext
    {
        public IDbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) 
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 
        }
    }
}