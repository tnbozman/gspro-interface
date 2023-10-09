using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrChangeDetection.Services
{
    public class ScreenCaptureService : IScreenCaptureService
    {
        public Bitmap CaptureRegion(Rectangle region)
        {
            var bmp = new Bitmap(region.Width, region.Height);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.CopyFromScreen(region.Left, region.Top, 0, 0, bmp.Size);
            }
            return bmp;
        }
    }
}
