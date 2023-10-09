using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrChangeDetection.Services
{
    public class BasicChangeDetectionService : IChangeDetectionService
    {
        public bool HasChanged(Bitmap current, ref Bitmap previous)
        {
            if (previous == null)
            {
                previous = current;
                return false;
            }

            bool hasChanged = !BitmapsAreEqual(previous, current);
            previous = current;
            return hasChanged;
        }

        private bool BitmapsAreEqual(Bitmap bmp1, Bitmap bmp2)
        {
            // Simplified pixel comparison
            if (bmp1.Size != bmp2.Size) return false;

            for (int y = 0; y < bmp1.Height; y++)
            {
                for (int x = 0; x < bmp1.Width; x++)
                {
                    if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y)) return false;
                }
            }
            return true;
        }
    }
}
