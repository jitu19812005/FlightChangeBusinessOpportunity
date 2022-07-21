using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {

            // call function here
            //
            try
            {

                if (args.Length == 3)
                {
                    DateTime StartDate, EndDate;
                    int AgencyId = 0;
                   
                    //Console.Write("Enter a Start date(yyyy-mm-dd) - ");
                    if (!DateTime.TryParseExact(args[0].ToString(), "yyyy-MM-dd",
                               CultureInfo.InvariantCulture,
                               DateTimeStyles.None, out StartDate))
                    {

                        Console.Write("\n Plesae enter correct date format");

                    }
                   // Console.Write("\n Enter a End date(yyyy-mm-dd) - ");
                    if (!DateTime.TryParseExact(args[1].ToString(), "yyyy-MM-dd",
                               CultureInfo.InvariantCulture,
                               DateTimeStyles.None, out EndDate))
                    {
                        Console.Write("\n Plesae enter correct date format");

                    }


                    //Console.Write("\n Enter a AgencyId - ");
                    if (!int.TryParse(args[2].ToString(), out AgencyId))
                    {
                        Console.Write("\n Plesae enter AgencyID in number");

                    }

                    var serviceshelper = new ServiceCollection();
                    ConfigureServices(serviceshelper);

                    var newAndDiscontinuedResult = serviceshelper.AddSingleton<ExecutorProcess, ExecutorProcess>().BuildServiceProvider().GetService<ExecutorProcess>().ExecuteThread(StartDate, EndDate, AgencyId);


                    Console.WriteLine("flight_id" + "       origin_city_id" + "      destination_city_id" + "       departure_time" + "                arrival_time" + "                        airline_id" + "                status");

                    if (newAndDiscontinuedResult.ToList().Count > 0)
                    {
                        foreach (MDLChangeDetectionController mdlChangeDetectionRow in newAndDiscontinuedResult.ToList())
                        {
                            Console.WriteLine("" + mdlChangeDetectionRow.flight_id.ToString() + "             " + mdlChangeDetectionRow.origin_city_id.ToString() + "                       " + mdlChangeDetectionRow.destination_city_id.ToString()
                                             + "             " + mdlChangeDetectionRow.departure_time + "          " + mdlChangeDetectionRow.arrival_time + "              " + mdlChangeDetectionRow.airline_id.ToString()
                                             + "                        " + mdlChangeDetectionRow.status.ToString()
                                              );


                        }
                    }
                    else
                    {
                        Console.WriteLine("\n No such flights has found in current filter");
                    }
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("\n Please enter correct value StartDate(yyyy-mm-dd)  EndDate(yyyy-mm-dd)  AgencyID(any number)");
                }
                }
            
            
            catch (Exception exc)
            {
                Console.Error.WriteLine(exc.ToString());
            }


            //test();


        }
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IChangeDetectionService, ChangeDetectionService>()
                .AddSingleton<IChangeDetectionRepository, ChangeDetectionRepository>();
        }
    }
}
        
