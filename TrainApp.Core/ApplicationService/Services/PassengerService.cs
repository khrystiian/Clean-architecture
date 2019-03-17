using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using TrainApp.Core.ApplicationService.Mapping;
using TrainApp.Core.Entity;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace TrainApp.Core.ApplicationService.Services
{
    public class PassengerService : IPassengerService
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();//log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IRepository<Passenger> _passengerRepo;
        private readonly IRepository<PassengersAge> _pAge;
        
        public PassengerService(IRepository<Passenger> passengerRepo, IRepository<PassengersAge> pAge)
        {
            _passengerRepo = passengerRepo ?? throw new ArgumentNullException(nameof(_passengerRepo));
            _pAge = pAge ?? throw new ArgumentNullException(nameof(_pAge));
        }

        public int GetPassengerId(string email)
        {
            try
            {
                if (email != null)
                {
                   return _passengerRepo.FindByEmail(email).CID;
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return default(int);
        }

        public void AddPassenger(PassengerModel passengerModel)
        {
            try
            {
                if (passengerModel == null)
                {
                    log.Debug("No passengers were added.");
                }
                else
                {
                    var p = PassengerMapping.MapPassengerModel(passengerModel);
                    _passengerRepo.Add(p);
                }
            }
            catch (Exception e)
            {
                log.Fatal(e);
            }
        }

        public IList<PassengerModel> ReadAllPassengers()
        {
            var list = new List<PassengerModel>();
            try
            {
                var all = _passengerRepo.GetAll();
                if (all.Count() <= 0)
                    log.Debug("No passengers were found.");

                foreach (var passenger in all)
                {
                    list.Add(PassengerMapping.MapPassenger(passenger));  //Convert domain objects to DTO
                }
            }
            catch (Exception e)
            {
                log.Fatal(e);
            }
            return list;
        }

        public void AddList(List<PassengersAge> pAges)
        {
            try
            {
                if (pAges.Count == 0)
                {
                    log.Debug("No passengers were added.");
                }
                else
                {
                    _pAge.AddList(pAges);
                }
            }
            catch (Exception e)
            {
                log.Fatal(e);
            }
        }

        public PassengerModel GetPassengerByEmail(string email)
        {

            if (email == null)
            {
                log.Debug("Email is null.");
            }
            var passenger =  _passengerRepo.FindByEmail(email);
            return PassengerMapping.MapPassenger(passenger);
        }
    }
}
