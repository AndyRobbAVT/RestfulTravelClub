using System;
using System.Linq;
using Model;

namespace Tests.Fakes
{
    public class FakeTripSet : FakeDbSet<Trip>
    {
        public override Trip Find(params object[] keyValues)
        {
            return this.SingleOrDefault(x => x.Id == (Guid)keyValues.Single());
        }
    }
}