using System;
using System.Collections.Generic;
using System.Data.Entity;
using Model;
using Repository;

namespace UnitTests
{
    public class InstrumContextInitializerForTests : DropCreateDatabaseAlways<InstrumEntitiesContext>
    {
        protected override void Seed(InstrumEntitiesContext context)
        {
            new List<Instrument>
            {
                new Instrument { Id = new Guid("1D37D829-AA79-4A26-911A-6AC68743F233"), Name =new InstrumentName(){Name= "Volvo B"}, ISIN = "S12345678910", MIC = "XSTO", CurrencyCode = "SEK", LifeCycleStatus = 1 },
                new Instrument { Id = new Guid("6D37D829-AA79-4A26-911A-6AC68743F233"), Name = new InstrumentName(){Name="Volvo B"}, ISIN = "S12345678910", MIC = "XSTO", CurrencyCode = "SEK", LifeCycleStatus = 1 }
            }.ForEach(x => context.Instruments.Add(x));

            new List<InstrumentSubscription>
            {
                new InstrumentSubscription { InstrumentId = new Guid("3D37D829-AA79-4A26-911A-6AC68743F233"),SystemId = new Guid("2D37D829-AA79-4A26-911A-6AC68743F233")}
            }.ForEach(instrumentSubscription => context.InstrumentSubscriptions.Add(instrumentSubscription));

            var portiaSystem = new ExternalSystem { Id = new Guid("5D37D829-AA79-4A26-911A-6AC68743F233") };
            context.ExternalSystems.Add(portiaSystem);

            context.SaveChanges();
        }
    }

}