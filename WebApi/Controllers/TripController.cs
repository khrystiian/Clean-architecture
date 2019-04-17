using System;
using System.Web.Http;
using TrainApp.Core.ApplicationService.Services;
using TrainApp.Core.DomainService;
using TrainApp.Core.Entity;

namespace UI.Controllers
{
    public class TripController : ApiController
    {
        private readonly PriceLogic _rootService;
        private readonly IElasticsearchLogic elasticsearch;

        public TripController(PriceLogic rootService, IElasticsearchLogic _elasticsearch)
        {
            _rootService = rootService ?? throw new ArgumentNullException(nameof(_rootService));
            elasticsearch = _elasticsearch ?? throw new ArgumentNullException(nameof(elasticsearch));
        }

        //GET:api/Trip/trip
        [HttpGet]
        public string Get(string search)
        {
            return elasticsearch.IndexSearch(search);
        }

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
