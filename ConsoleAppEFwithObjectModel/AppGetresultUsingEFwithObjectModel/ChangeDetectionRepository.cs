using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace ConsoleApp4
{

    public class ChangeDetectionRepository : IChangeDetectionRepository
    {
        
        public List<object> GetAllNewAndDiscontinuedFlights(DateTime StartDate, DateTime EndDate, int AgencyId)
        {
           
            return ChangeDetectionAlgorithemEFWithDBTableObject(StartDate, EndDate, AgencyId);
            
        }

       private List<object> ChangeDetectionAlgorithemEFWithDBTableObject(DateTime StartDate, DateTime EndDate, int AgencyId)
        {
            try
            {
                
                if (StartDate != default(DateTime) || EndDate != default(DateTime) || AgencyId != 0)
                {
                    using (BAU_EP2MY_SITEntities dbContext = new BAU_EP2MY_SITEntities())
                    {
                        dbContext.Database.CommandTimeout = 1800;
                        // date filter

                        // select f1.*into temp_f
                        //from[Temp - f1] f1
                        //where
                        //f1.departure_time
                        //between dateadd(day, -7, dateadd(minute, -30, @startdt)) and
                        //dateadd(day, +7, dateadd(minute, +30, @enddt))
                        var flightsDTFilter = from p in dbContext.flights
                                                  //   where p.departure_time >= StartDate.Date.AddDays(-7).AddMinutes(-30) && p.departure_time <= EndDate.Date.AddDays(8).AddMinutes(30)
                                              where p.departure_time >= DbFunctions.AddMinutes(DbFunctions.AddDays(StartDate, -7), -30)
                                              && p.departure_time <= DbFunctions.AddMinutes(DbFunctions.AddDays(EndDate, 8), 30)
                                              select p;

                        // Ontime Flights filter 

                        //                    select f1.route_id, f1.flight_id, r.origin_city_id ,r.destination_city_id ,
                        //f1.departure_time , f1.arrival_time ,f1.airline_id , r.departure_date,s.agency_id, 'ontime' status into  temp_ontime
                        //from temp_f f1, [Temp - r1] r
                        //, [dbo].[temp - s] s
                        //  where f1.route_id = r.route_id
                        //and r.origin_city_id = s.origin_city_id
                        //and r.destination_city_id = s.destination_city_id
                        //and f1.departure_time between   @startdt and  @enddt
                        //and s.agency_id = @agencyid
                        var onTimeFlights = from f in flightsDTFilter
                                            join r in dbContext.routes on f.route_id equals r.route_id
                                            join s in dbContext.subscriptions on new { r.origin_city_id, r.destination_city_id } equals new { s.origin_city_id, s.destination_city_id }
                                            where f.departure_time >= StartDate && f.departure_time <= DbFunctions.AddDays(EndDate, 8) && s.agency_id == AgencyId
                                            select new
                                            {
                                                f.route_id,
                                                f.flight_id,
                                                r.origin_city_id,
                                                r.destination_city_id,
                                                f.departure_time,
                                                f.arrival_time,
                                                f.airline_id,
                                                r.departure_date,
                                                s.agency_id
                                            };

                        // New flights


                        //                    select f1.route_id, f1.flight_id, r.origin_city_id ,r.destination_city_id ,
                        //f1.departure_time , f1.arrival_time ,f1.airline_id , r.departure_date,s.agency_id ,'New' status from  temp_f f1,(select flight_id,
                        //route_id, airline_id, origin_city_id, destination_city_id,
                        //dateadd(day, -7, dateadd(minute, -30, departure_time)) startdt, dateadd(day, -7, dateadd(minute, +30, departure_time)) enddt
                        //, departure_time, arrival_time
                        //from temp_ontime
                        //--where
                        //--flight_id = 1321941
                        //) t , [Temp-r1] r , [dbo].[temp - s] s where
                        //f1.route_id = r.route_id and
                        //t.origin_city_id = r.origin_city_id and
                        //t.destination_city_id = r.destination_city_id and
                        //r.origin_city_id = s.origin_city_id and
                        //r.destination_city_id = s.destination_city_id
                        //and f1.departure_time between  t.startdt and t.enddt
                        //and f1.airline_id<> t.airline_id
                        //and s.agency_id = @agencyid

                        var newFlights = (from f1 in flightsDTFilter
                                         join r in dbContext.routes on f1.route_id equals r.route_id
                                         join s in dbContext.subscriptions on new { r.origin_city_id, r.destination_city_id } equals new { s.origin_city_id, s.destination_city_id }
                                         join t in onTimeFlights on new { r.origin_city_id, r.destination_city_id } equals new { t.origin_city_id, t.destination_city_id }
                                         where f1.departure_time >= DbFunctions.AddMinutes(DbFunctions.AddDays(t.departure_time, -7), -30) && f1.departure_time <= DbFunctions.AddMinutes(DbFunctions.AddDays(t.departure_time, -7), 30)
                                               && f1.airline_id != t.airline_id && s.agency_id == AgencyId
                                         select new MDLChangeDetectionController
                                         {
                                             route_id = f1.route_id,
                                             flight_id = f1.flight_id,
                                             origin_city_id = r.origin_city_id,
                                             destination_city_id = r.destination_city_id,
                                             departure_time = f1.departure_time,
                                             arrival_time = f1.arrival_time,
                                             airline_id = f1.airline_id,
                                             departure_date = r.departure_date,
                                             agency_id = s.agency_id,
                                             status = "New"
                                         }).Distinct();



                        //Discontinued flights

                        //                    select f1.route_id, f1.flight_id, r.origin_city_id ,r.destination_city_id ,
                        //f1.departure_time , f1.arrival_time ,f1.airline_id , r.departure_date,s.agency_id ,'Discontinued' status from  temp_f f1,(select flight_id,
                        //route_id, airline_id, origin_city_id, destination_city_id,
                        //dateadd(day, +7, dateadd(minute, -30, departure_time)) startdt, dateadd(day, +7, dateadd(minute, +30, departure_time)) enddt
                        //, departure_time, arrival_time
                        //from temp_ontime
                        //--where
                        //--flight_id = 1321941
                        //) t , [Temp-r1] r , [dbo].[temp - s] s where
                        //f1.route_id = r.route_id and
                        //t.origin_city_id = r.origin_city_id and
                        //t.destination_city_id = r.destination_city_id and
                        //r.origin_city_id = s.origin_city_id and
                        //r.destination_city_id = s.destination_city_id
                        //and f1.departure_time between  t.startdt and t.enddt
                        //and f1.airline_id<> t.airline_id
                        //and s.agency_id = @agencyid


                        var discontinuedFlights = (from f1 in flightsDTFilter
                                                  join r in dbContext.routes on f1.route_id equals r.route_id
                                                  join s in dbContext.subscriptions on new { r.origin_city_id, r.destination_city_id } equals new { s.origin_city_id, s.destination_city_id }
                                                  join t in onTimeFlights on new { r.origin_city_id, r.destination_city_id } equals new { t.origin_city_id, t.destination_city_id }
                                                  where f1.departure_time >= DbFunctions.AddMinutes(DbFunctions.AddDays(t.departure_time, 7), -30) && f1.departure_time <= DbFunctions.AddMinutes(DbFunctions.AddDays(t.departure_time, 7), 30)
                                                        && f1.airline_id != t.airline_id && s.agency_id == AgencyId
                                                  select new MDLChangeDetectionController
                                                  {
                                                      route_id =f1.route_id,
                                                      flight_id =f1.flight_id,
                                                      origin_city_id = r.origin_city_id,
                                                      destination_city_id =r.destination_city_id,
                                                      departure_time =f1.departure_time,
                                                      arrival_time =f1.arrival_time,
                                                      airline_id =f1.airline_id,
                                                      departure_date =r.departure_date,
                                                      agency_id =s.agency_id,
                                                      status = "Discontinued"
                                                  }).Distinct();

                        return newFlights.Union(discontinuedFlights).ToList<object>();
                        //var NewnDiscontinuedUnion = newFlights.Union(discontinuedFlights).ToList();


                       
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }


            

        }
    }

}


            
       

   

