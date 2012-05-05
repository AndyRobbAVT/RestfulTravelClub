using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model;

namespace Api.Controllers
{
    public class TripsController : ApiController
    {
        private readonly ITravelClubEntitiesContext _dbContext;
        public TripsController(ITravelClubEntitiesContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Trip> Get()
        {
            return _dbContext.Trips;
        }

        public HttpResponseMessage<Trip> Get(Guid id)
        {
            Trip Trip = _dbContext.Trips.SingleOrDefault(x => x.Id == id);
            if (Trip == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return new HttpResponseMessage<Trip>(Trip, HttpStatusCode.OK);
        }

        public HttpResponseMessage Post(Trip Trip)
        {
            Trip addedTrip = _dbContext.Trips.Add(Trip);
            var response = new HttpResponseMessage<Trip>(HttpStatusCode.Created);
            response.Headers.Location = new Uri(ControllerContext.Request.RequestUri + addedTrip.Id.ToString());
            return response;
        }

        public HttpResponseMessage Put(Guid id, Trip trip)
        {
            if(_dbContext.Trips.SingleOrDefault(x=>x.Id==id)== null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            _dbContext.Trips.AddOrUpdate(trip);
            _dbContext.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage<Trip> Delete(Guid id)
        {
            var toDelete = _dbContext.Trips.SingleOrDefault(x => x.Id == id);
            if(toDelete == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            _dbContext.Trips.Remove(toDelete);
            return new HttpResponseMessage<Trip>(toDelete, HttpStatusCode.OK);
        }
    }
}