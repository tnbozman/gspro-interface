using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarminInterface.Models.ToR10
{
    public class ChallengeAcknowledgeDto
    {
        // {"Success":"true","Type":"Authentication"}
        [JsonProperty("Success")]
        public string Success { get; set; }
        [JsonProperty("Type")]
        public string Type { get; set; }
        
    }
}
