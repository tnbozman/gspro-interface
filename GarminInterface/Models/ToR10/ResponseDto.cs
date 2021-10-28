using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarminInterface.Models.ToR10
{
    public class ResponseDto
    {

        [JsonProperty("Type")]
        public string Type { get; set; }
        [JsonProperty("SubType")]
        public string? SubType { get; set; }
        [JsonProperty("Details")]
        public string Details { get; set; }

    }
}
