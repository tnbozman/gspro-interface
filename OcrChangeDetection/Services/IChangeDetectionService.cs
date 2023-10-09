using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrChangeDetection.Services
{
    public interface IChangeDetectionService
    {
        bool HasChanged(Bitmap current, ref Bitmap previous);
    }
}
