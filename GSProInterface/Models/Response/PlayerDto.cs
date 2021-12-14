using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models.Reponse
{
    public class PlayerDto
    {
        [JsonProperty("Handed")]
        public string Handed { get; set; }
        [JsonProperty("Club")]
        public string Club { get; set; }
        [JsonProperty("DistanceToPin")]
        public float? DistanceToPin { get; set; }
    }
}
