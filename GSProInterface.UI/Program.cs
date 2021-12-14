using GSProInterface.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSProInterface.UI
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // create the service collection
            var services = new ServiceCollection();
            // configure the service collection and service provider
            ConfigureServices(services);
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            
            // get the required services to run the Winform application
            IGSProInterface app = serviceProvider.GetService<IGSProInterface>();
            ILogger<GSPRO_INTERFACE> logger = (ILogger<GSPRO_INTERFACE>) serviceProvider.GetService<ILoggerFactory>().CreateLogger<GSPRO_INTERFACE>();

            Application.Run(new GSPRO_INTERFACE(app, logger));
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Error).AddConsole());
            services.AddTransient<IStreamClient, StreamClientAdvanced>();
            services.AddTransient<IGSProInterface, GSProStreamInterface>();
            services.AddTransient<IDeviceDetails>(s => new DeviceDetails { DeviceName = "Example LM" });
        }
    }
}
