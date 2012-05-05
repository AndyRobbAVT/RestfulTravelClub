using System;
using System.Linq;
using Model;

namespace UnitTests.Fakes
{
    public class FakeInstrumentSubscriptionSet : FakeDbSet<InstrumentSubscription>
    {
        public override InstrumentSubscription Find(params object[] keyValues)
        {
            return this.SingleOrDefault(x => x.InstrumentId == (Guid)keyValues.Single() && x.SystemId == (Guid)keyValues.Single());
        }
    }

    public class FakeInstrumentSubscriptionRequestSet : FakeDbSet<InstrumentSubscriptionRequest>
    {
        public override InstrumentSubscriptionRequest Find(params object[] keyValues)
        {
            return this.SingleOrDefault(x => x.Id == (Guid)keyValues.Single());
        }
    }
}