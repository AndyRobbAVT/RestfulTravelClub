using System;
using Infrastructure;
using Model;
using NUnit.Framework;
using UnitTests.Fakes;

namespace UnitTests
{
    [TestFixture]
    public class UnitTestBase
    {
        protected IInstrumEntitiesContext context;
        [SetUp]
        public void SetUp()
        {
            SetUpDb();
        }

        private void SetUpDb()
        {
            context = new FakeInstrumEntitiesContext
            {
                Instruments =
                        {
                            new Instrument
                                {
                                    Id =new Guid("1D37D829-AA79-4A26-911A-6AC68743F233"),
                                    Name = new InstrumentName()
                                            {
                                                Name = "Volvo B"
                                            },
                                    ISIN = "S12345678910",
                                    MIC = "XSTO",
                                    CurrencyCode = "SEK",
                                    LifeCycleStatus = 1
                                },
                            new Instrument
                                {
                                    Id =new Guid("6D37D829-AA79-4A26-911A-6AC68743F233"),
                                    Name = new InstrumentName()
                                            {
                                                Name = "Ericsson B"
                                            },
                                    ISIN = "S12345678911",
                                    MIC = "XSTO",
                                    CurrencyCode = "SEK",
                                    LifeCycleStatus = 2
                                }
                        }
            };
        }
    }
}