using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class MDLChangeDetectionController
    {
       
        public double flight_id { get; set; }

        public Nullable<double> agency_id { get; set; }
        public string status { get; set; }
        public Nullable<double> route_id { get; set; }
        public Nullable<System.DateTime> departure_time { get; set; }
        public Nullable<System.DateTime> arrival_time { get; set; }
        public Nullable<double> airline_id { get; set; }
        public Nullable<double> origin_city_id { get; set; }
        public Nullable<double> destination_city_id { get; set; }
        public Nullable<System.DateTime> departure_date { get; set; }
    }
}

