using System;
using System.Net;
using System.Net.Http;
using Machine.Specifications;
using Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class When_requesting_an_existing_trip : TestBase
    {
        [Test]
        public void It_should_return_the_trip()
        {
            var trip = client.Get<Trip>(Constants.Existing_Trip_1);
            trip.Id.ToString().ToLower().ShouldEqual(Constants.Existing_Trip_1);
        }
    }

    [TestFixture]
    public class When_posting_a_new_trip : TestBase
    {
        Trip trip;
        Guid id = Guid.NewGuid();
        HttpResponseMessage response;

        [SetUp]
        public void Context()
        {
            trip = new Trip { Id = id, Name = "New York spring tour" };
            response = client.Post(trip);
        }

        [Test]
        public void It_should_set_the_correct_location()
        {
            response.Headers.Location.ToString().ShouldEqual("http://localhost:8080/api/trips/" + id);
            response.StatusCode.ShouldEqual(HttpStatusCode.Created);
        }
    }
}
