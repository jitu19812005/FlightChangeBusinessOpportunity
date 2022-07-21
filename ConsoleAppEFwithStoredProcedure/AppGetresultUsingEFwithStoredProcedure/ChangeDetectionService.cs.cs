using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class ChangeDetectionService : IChangeDetectionService
    {
        private readonly IChangeDetectionRepository _IChangeDetectionRepo;

        public ChangeDetectionService(IChangeDetectionRepository ChangeDetectionRepo)
        {
            _IChangeDetectionRepo = ChangeDetectionRepo;
        }
     

        public IList<object>  GetAllNewAndDiscontinuedFlights(DateTime StartDate, DateTime EndDate, int AgencyId)
        {
        return _IChangeDetectionRepo.GetAllNewAndDiscontinuedFlights(StartDate, EndDate, AgencyId);
        }

       

    }
}
