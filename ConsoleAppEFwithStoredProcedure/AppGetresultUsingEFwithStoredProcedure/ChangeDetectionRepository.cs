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
           
           
            return ChangeDetectionAlgorithemEFWithStoreProcedure(StartDate, EndDate, AgencyId).ToList<object>();
        }

        private List<MDLChangeDetectionController> ChangeDetectionAlgorithemEFWithStoreProcedure(DateTime StartDate, DateTime EndDate, int AgencyId)
        {
            List<MDLChangeDetectionController> MDLChangeDetectionLst = new List<MDLChangeDetectionController>();
            using (BAU_EP2MY_SITEntities dbContext = new BAU_EP2MY_SITEntities())
            {
                // 2. Retrieve the OAConnection instance.
                using (IDbConnection oaConnection = dbContext.Database.Connection)
                {
                    // 3. Create a new instance of the OACommand class.
                    oaConnection.Open();
                    using (IDbCommand oaCommand = oaConnection.CreateCommand())
                    {
                        // 4. Set the CommandType property.
                        oaCommand.CommandType = CommandType.StoredProcedure;

                        // 5. Set the CommandText property.
                        oaCommand.CommandText = "getReasultUsingChangeDetectionAlgo";
                        oaCommand.Parameters.Add(new SqlParameter { ParameterName = "@startdt", Value = StartDate });
                        oaCommand.Parameters.Add(new SqlParameter { ParameterName = "@enddt", Value = EndDate });
                        oaCommand.Parameters.Add(new SqlParameter { ParameterName = "@agencyid", Value = AgencyId });

                        // 6. Execute the command and materialize the car entities
                        using (IDataReader dataReader = oaCommand.ExecuteReader())
                        {
                          
                            while (dataReader.Read())
                            {
                                MDLChangeDetectionLst.Add(new MDLChangeDetectionController
                                {
                                    agency_id = Convert.ToDouble(dataReader["agency_id"].ToString()),
                                    flight_id = Convert.ToDouble(dataReader["flight_id"].ToString()),
                                    departure_time = Convert.ToDateTime(dataReader["departure_time"].ToString()),
                                    arrival_time = Convert.ToDateTime(dataReader["arrival_time"].ToString()),
                                    airline_id = Convert.ToDouble(dataReader["airline_id"].ToString()),
                                    origin_city_id = Convert.ToDouble(dataReader["origin_city_id"].ToString()),
                                    destination_city_id = Convert.ToDouble(dataReader["destination_city_id"].ToString()),
                                    status = dataReader["status"].ToString(),
                                });
                            }

                            dataReader.Close();
                        }
                        //Release resources  

                        
                    }
                    oaConnection.Close();
                }
            }
            return MDLChangeDetectionLst;

        }

        
    }

}


            
       

   

