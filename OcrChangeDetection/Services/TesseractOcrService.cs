using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace OcrChangeDetection.Services
{
    public class TesseractOcrService : IOcrService
    {
        public string ProcessImage(Bitmap image)
        {
            using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
            using var pix = PixConverter.ToPix(image);
            using var page = engine.Process(pix);
            return page.GetText();
        }
    }
}
