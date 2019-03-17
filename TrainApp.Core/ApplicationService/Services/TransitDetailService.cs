using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using TrainApp.Core.ApplicationService.Interfaces;

namespace TrainApp.Core.ApplicationService.Services
{
    public class TransitDetailService: ITransitService
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly IRepository<TransitDetail> _tRepo;

        public TransitDetailService(IRepository<TransitDetail> tRepo) => _tRepo = tRepo ?? throw new ArgumentNullException(nameof(_tRepo));

        public void AddList(List<TransitDetail> tDetails)
        {
            try
            {
                if (tDetails.Count == 0)
                {
                    log.Debug("No passengers were added.");
                }
                else
                {
                    _tRepo.AddList(tDetails);
                }
            }
            catch (Exception e)
            {
                log.Fatal(e);
            }
        }
    }
}
