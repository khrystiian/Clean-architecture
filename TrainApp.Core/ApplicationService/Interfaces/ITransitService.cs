using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainApp.Core.ApplicationService.Interfaces
{
    public interface ITransitService
    {
        void AddList(List<TransitDetail> td);
    }
}
