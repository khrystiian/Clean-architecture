using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Web.Http.Cors;
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

        //GET:api/Trip/trip
        //public LegModel Get(string id)
        //{
        //    return _rootService.GetTripById(id);
        //}

        // POST: api/Trip
        public TripResponseModel Post([FromBody]RootObjectAPI tripRootObject)
        {
            var json = JsonConvert.SerializeObject(tripRootObject);
            if (tripRootObject == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NoContent);
            }
            return _rootService.TripPrice(tripRootObject);
        }

        // PUT: api/Trip/5
        [HttpPut]
        public bool Put(string id, [FromBody]RootObjectAPI root)
        {
            return _rootService.ConfirmTrip(id, root.Status);
        }

        // DELETE: api/Trip/5
        public void Delete(int id) { }
    }
}
