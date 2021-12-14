using GSProInterface.Models.Request;
using GSProInterface.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Sockets;

namespace GSProInterface.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // create the service collection
            var services = new ServiceCollection();
            // configure the service collection and service provider
            ConfigureServices(services);

            // get the required services to start the console application
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            IGSProInterface app = serviceProvider.GetService<IGSProInterface>();

            StartClient(app);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Error).AddConsole());
            services.AddTransient<IStreamClient, StreamClientAdvanced>();
            services.AddTransient<IGSProInterface, GSProStreamInterface>();
            services.AddTransient<IDeviceDetails>(s => new DeviceDetails { DeviceName = "Example LM" });
        }

        private static void StartClient(IGSProInterface gsPro)
        {
            Console.WriteLine("Shot test application stating");
            Console.WriteLine("BallData support only, does not support club data");
            try
            {
                gsPro.StartClient("127.0.0.1", 0921);
                try
                {
                    gsPro.SendLaunchMonitorStatus(true);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
                ConsoleKeyInfo thisKey;
                Console.WriteLine("hit 'x' key to send shots");
                while (true)
                {
                    while (!Console.KeyAvailable)
                    {
                        // Do something, but don't read key here
                    }

                    //Thread.Sleep(1000);
                    thisKey = Console.ReadKey();
                    if(thisKey.KeyChar == 'q')
                    {
                        Console.WriteLine("q detected. exiting");
                        break;
                    }else if(thisKey.KeyChar != 'x')
                    {
                        continue;
                    }
                    Console.WriteLine(" key hit - sending shot");

                    var ballData = new BallDataDto
                    {
                        Speed = 147.5f * RandomNumberBetween(0, 1),
                        SpinAxis = -13.2f * RandomNumberBetween(0, 1),
                        TotalSpin = 3250f * RandomNumberBetween(0, 1),
                        BackSpin = 2500f * RandomNumberBetween(0, 1),
                        SideSpin = -800f * RandomNumberBetween(0, 1),
                        HLA = 2.3f * RandomNumberBetween(0, 1),
                        VLA = 14.3f * RandomNumberBetween(0, 1),
                        CarryDistance = 256.5f * RandomNumberBetween(0, 1)
                    };

                    try
                    {
                        var response = gsPro.SendBallData(ballData);
                        // Write the response to the console.  
                        Console.WriteLine("Response received : {0}", response.Code);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                }

                gsPro.StopClient();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        private static readonly Random random = new Random();
        private static float RandomNumberBetween(double minValue, double maxValue)
        {
            var next = random.NextDouble();

            return (float)(minValue + (next * (maxValue - minValue)));
        }
    }
}
