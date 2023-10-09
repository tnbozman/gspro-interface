using OcrChangeDetection.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OcrChangeDetection.Services
{
    public class JsonConfigurationService : IConfigurationService
    {
        public Config ReadConfig(string path)
        {
            var jsonString = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Config>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
