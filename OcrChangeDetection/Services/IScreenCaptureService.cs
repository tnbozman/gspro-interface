using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrChangeDetection.Services
{
    public interface IScreenCaptureService
    {
        Bitmap CaptureRegion(Rectangle region);
    }
}
