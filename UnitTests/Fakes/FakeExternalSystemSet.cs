using System;
using System.Linq;
using Model;

namespace UnitTests.Fakes
{
    public class FakeExternalSystemSet : FakeDbSet<ExternalSystem>
    {
        public override ExternalSystem Find(params object[] keyValues)
        {
            return this.SingleOrDefault(x => x.Id == (Guid)keyValues.Single());
        }
    }
}