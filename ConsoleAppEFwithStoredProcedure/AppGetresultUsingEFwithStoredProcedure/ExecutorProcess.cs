using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class ExecutorProcess
    {
        readonly IChangeDetectionService _ChangeDetectionService;

        public ExecutorProcess(IChangeDetectionService ChangeDetectionService)
        {
            _ChangeDetectionService = ChangeDetectionService;
        }

        public IList<object> ExecuteThread(DateTime StartDate, DateTime EndDate, int AgencyId)
        {
            return _ChangeDetectionService.GetAllNewAndDiscontinuedFlights(StartDate, EndDate, AgencyId);
        }
        }
    }
