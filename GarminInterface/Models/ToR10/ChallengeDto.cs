using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarminInterface.Models.ToR10
{
    // {"Challenge": "gQW3om37uK4OOU4FXQH9GWgljxOrNcL5MvubVHAtQC0x6Z1AwJTgAIKyamJJMzm9", "E6Version": "2, 0, 0, 0", "ProtocolVersion": "1.0.0.5", "RequiredProtocolVersion": "1.0.0.0", "Type": "Handshake"}
    public class ChallengeDto
    {
        [JsonProperty("Challenge")]
        public string Challenge { get; set; }
        [JsonProperty("E6Version")]
        public string E6Version { get; set; }
        [JsonProperty("ProtocolVersion")]
        public string ProtocolVersion { get; set; }
        [JsonProperty("RequiredProtocolVersion")]
        public string RequiredProtocolVersion { get; set; }
        [JsonProperty("Type")]
        public string Type { get; set; }
    }
}
