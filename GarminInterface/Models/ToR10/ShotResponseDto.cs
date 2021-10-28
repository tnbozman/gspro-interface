using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarminInterface.Models.ToR10
{
    public class ShotResponseDto
    {
        [JsonProperty("Details")]
        public ShotDetailsDto Details { get; set; }
        [JsonProperty("Type")]
        public string Type { get; set; }
        [JsonProperty("SubType")]
        public string SubType { get; set; }
    }

}