using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OcrChangeDetection.Services;
using System;

namespace OcrChangeDetectConsole // Note: actual namespace depends on the project name.
{
    static class Program
    {
        static void Main(string[] args)
        {
            // create the service collection
            var services = new ServiceCollection();
            // configure the service collection and service provider
            ConfigureServices(services);
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var appService = serviceProvider.GetRequiredService<IOcrChangeDetectionService>();
            appService.Run();
        }
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Error).AddConsole());
            services.AddTransient<IChangeDetectionService, BasicChangeDetectionService>();
            services.AddTransient<IConfigurationService, JsonConfigurationService>();
            services.AddTransient<IOcrChangeDetectionService, OcrChangeDetectionService>();
            services.AddTransient<IOcrService, TesseractOcrService>(); 
            services.AddTransient<IScreenCaptureService, ScreenCaptureService>();
        }
    }

}
