using System;
using System.Linq;
using Model;

namespace UnitTests.Fakes
{
    public class FakeDepartmentSet : FakeDbSet<Instrument>
    {
        public override Instrument Find(params object[] keyValues)
        {
            return this.SingleOrDefault(x => x.Id == (Guid)keyValues.Single());
        }
    }
}