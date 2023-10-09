using OcrChangeDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrChangeDetection.Services
{
    public interface IConfigurationService
    {
        Config ReadConfig(string path);
    }
}
