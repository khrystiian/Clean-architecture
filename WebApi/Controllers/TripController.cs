using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Web.Http.Cors;
using TrainApp.Core.ApplicationService.Services;
using TrainApp.Core.Entity;

namespace UI.Controllers
{
    public class TripController : ApiController
    {
        private readonly PriceLogic _rootService;

        public TripController(PriceLogic rootService)
        {
            _rootService = rootService ?? throw new ArgumentNullException(nameof(_rootService));
        }

        //GET:api/Trip/trip
        //public LegModel Get(string id)
        //{
        //    return _rootService.GetTripById(id);
        //}

        [HttpPost]
        // POST: api/Trip
        public TripResponseModel Post([FromBody]RootObjectAPI tripRootObject)
        {
            if (tripRootObject != null)
            {
                return _rootService.TripPrice(tripRootObject);
            }
            else
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NoContent);
            }

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
