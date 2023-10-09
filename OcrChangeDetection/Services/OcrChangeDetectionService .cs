using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrChangeDetection.Services
{
    public class OcrChangeDetectionService: IOcrChangeDetectionService
    {
        private readonly IScreenCaptureService _captureService;
        private readonly IOcrService _ocrService;
        private readonly IConfigurationService _configService;
        private readonly IChangeDetectionService _changeDetectionService;

        public OcrChangeDetectionService(IScreenCaptureService captureService, IOcrService ocrService, IConfigurationService configService, IChangeDetectionService changeDetectionService)
        {
            _captureService = captureService;
            _ocrService = ocrService;
            _configService = configService;
            _changeDetectionService = changeDetectionService;
        }

        public void Run()
        {
            var config = _configService.ReadConfig(SystemConstants.CONFIG_PATH);
            Bitmap previousTrigger = null;

            while (true)
            {
                var triggerCapture = _captureService.CaptureRegion(new Rectangle(config.Trigger.X, config.Trigger.Y, config.Trigger.Width, config.Trigger.Height));

                if (_changeDetectionService.HasChanged(triggerCapture, ref previousTrigger))
                {
                    foreach (var region in config.Regions)
                    {
                        var regionCapture = _captureService.CaptureRegion(new Rectangle(region.X, region.Y, region.Width, region.Height));
                        var result = _ocrService.ProcessImage(regionCapture);
                        Console.WriteLine($"{region.Label}: {result}");
                    }
                }

                Thread.Sleep(1000);
            }
        }
    }
}
