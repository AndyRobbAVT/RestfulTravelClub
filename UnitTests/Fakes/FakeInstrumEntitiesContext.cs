using System.Data.Entity;
using Infrastructure;
using Model;

namespace UnitTests.Fakes
{

    public class FakeInstrumEntitiesContext : IInstrumEntitiesContext
    {
        public FakeInstrumEntitiesContext()
        {
            this.Instruments = new FakeDepartmentSet();
            this.ExternalSystems = new FakeExternalSystemSet();
            this.InstrumentSubscriptions = new FakeInstrumentSubscriptionSet();
        }

        public IDbSet<Instrument> Instruments { get; set; }
        public IDbSet<ExternalSystem> ExternalSystems { get; set; }
        public IDbSet<InstrumentSubscription> InstrumentSubscriptions { get; set; }
        public IDbSet<InstrumentSubscriptionRequest> InstrumentSubscriptionRequests { get; set; }

        public int SaveChanges()
        {
            return 0;
        }
    }
}