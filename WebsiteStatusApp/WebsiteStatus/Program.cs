using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace WebsiteStatus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Set up Serilog:
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                 .Enrich.FromLogContext()
                 .WriteTo.File(@"C:\Temp\WorkerServiceTest\LogFile.txt")
                 .CreateLogger();




            try
            {
                Log.Information("Starting up the service");
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There wasa problem starting the service");
                return;

            }
            finally
            { //if there are any messages in the bugger they'll get written before we close out the application..messages will get written out incase of a crash etc
                Log.CloseAndFlush();
            }

            //CreateHostBuilder(args).Build().Run();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
              .UseWindowsService()
              .ConfigureServices((hostContext, services) =>

              {
                  services.AddHostedService<Worker>();
              })
              .UseSerilog();
         }
    }



// creates a logfile follow the path to see it. 