using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Machine.Specifications;
using Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class When_:TestBase
    {
        [Test]
        public void It_should()
        {
            var id = new Guid("CDC55BE1-4C80-49CC-9026-EEB16EAABB01");
            var trip = new WebApiClient(Constants.BaseUri + "api/trips/").Get<Trip>(id.ToString());
            trip.Id.ShouldEqual(id);
        }

        [Test]
        public void It_shouldx()
        {

            var webApiClient = new WebApiClient(Constants.BaseUri + "api/trips/");
            var result = webApiClient.Get<IEnumerable<Trip>>();
        }

        [Test]
        public void It_shouldy()
        {
            Guid id = Guid.NewGuid();
            var trip = new Trip {Id = id, Name = "New trip"};
            var response = new WebApiClient(Constants.BaseUri + "api/trips/").Post(trip);
            response.Headers.Location.ToString().ShouldEqual("http://localhost:8080/api/trips/" + id.ToString());
        }


    }
}
