using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using TrainApp.Core.ApplicationService;
using TrainApp.Core.Entity;


namespace UI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PassengerController : ApiController
    {
        private readonly IPassengerService _passengerService;
        
        public PassengerController(IPassengerService passengerService) 
        {
            this._passengerService = passengerService ?? throw new ArgumentNullException(nameof(_passengerService));
        }

        // GET: api/Passenger
        [HttpGet]
        public IList<PassengerModel> Get()
        {
            return _passengerService.ReadAllPassengers();
        }
        

        [HttpPost]
        // POST: api/Passenger
        public void Post([FromBody]PassengerModel passenger)
        {
            _passengerService.AddPassenger(passenger);           
            if (passenger == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NoContent);
        }

        [HttpGet]
        // GET: api/Passenger/5
        public PassengerModel Get(string email)
        {
            if (email == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            return _passengerService.GetPassengerByEmail(email);
        }

    }
}
