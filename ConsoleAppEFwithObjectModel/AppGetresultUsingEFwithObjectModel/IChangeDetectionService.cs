using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public interface IChangeDetectionService
    {
        IList<object> GetAllNewAndDiscontinuedFlights(DateTime StartDate, DateTime EndDate, int AgencyId);
       
    }
}
