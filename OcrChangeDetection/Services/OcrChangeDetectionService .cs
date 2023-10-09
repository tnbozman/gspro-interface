using OcrChangeDetection.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public void DrawRegionsOnBitmap(Bitmap bitmap, List<RegionConfig> regions, RegionConfig trigger)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                foreach (var region in regions)
                {
                    g.DrawRectangle(Pens.Green, region.X, region.Y, region.Width, region.Height);
                    g.DrawString(region.Label, SystemFonts.DefaultFont, Brushes.Green, region.X, region.Y - 20);
                }

                g.DrawRectangle(Pens.Yellow, trigger.X, trigger.Y, trigger.Width, trigger.Height);
                g.DrawString("Trigger", SystemFonts.DefaultFont, Brushes.Yellow, trigger.X, trigger.Y - 20);
            }
        }

        public Bitmap CaptureScreen()
        {
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(screenBounds.Width, screenBounds.Height);

            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, screenBounds.Size);
            }

            return screenshot;
        }

        public void SaveImageWithRegions(string path, List<RegionConfig> regions, RegionConfig trigger)
        {
            Bitmap screenshot = CaptureScreen();
            DrawRegionsOnBitmap(screenshot, regions, trigger);
            screenshot.Save(path, ImageFormat.Png);
        }

        public void Run()
        {
            var config = _configService.ReadConfig(SystemConstants.CONFIG_PATH);
            SaveImageWithRegions(SystemConstants.OVERLAY_OUTPUT, config.Regions, config.Trigger);

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
