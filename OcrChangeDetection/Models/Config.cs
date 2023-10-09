using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrChangeDetection.Models
{
    public class Config
    {
        public RegionConfig Trigger { get; set; }
        public List<RegionConfig> Regions { get; set; }
    }
}
