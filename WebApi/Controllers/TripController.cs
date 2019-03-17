using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using TrainApp.Core.ApplicationService.Interfaces;
using TrainApp.Core.ApplicationService.Services;
using TrainApp.Core.Entity;

namespace UI.Controllers
{
    // Allow CORS for all origins. 
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TripController : ApiController
    {
        private readonly PriceLogic _rootService;

        public TripController(PriceLogic rootService)
        {
            this._rootService = rootService ?? throw new ArgumentNullException(nameof(_rootService));
        }

        // GET: api/Trip/5
        public string Get(int id)
        {
            return "value";
        }

        //GET:api/Trip/trip
        //public Rootobject Get()
        //{
        //    return _tripService.GetRootObject();
        //}

        // POST: api/Trip
        public double Post([FromBody]RootObjectAPI tripRootObject)
        {
            var json = JsonConvert.SerializeObject(tripRootObject);
            if (tripRootObject == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NoContent);
            }
            return _rootService.TripPrice(tripRootObject);
        }

        // PUT: api/Trip/5
        public void Put(bool confirm)
        {
            _rootService.ConfirmTrip(confirm);
        }

        // DELETE: api/Trip/5
        public void Delete(int id)
        {
        }
    }
}
