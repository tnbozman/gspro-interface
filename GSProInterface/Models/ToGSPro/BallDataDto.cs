using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models.Request
{
    public class BallDataDto
    {
        [JsonProperty("Speed")]
        public float Speed { get; set; }
        [JsonProperty("SpinAxis")]
        public float SpinAxis { get; set; }
        [JsonProperty("TotalSpin")]
        public float TotalSpin { get; set; }
        [JsonProperty("BackSpin")]
        public float? BackSpin { get; set; }
        [JsonProperty("SideSpin")]
        public float? SideSpin { get; set; }
        [JsonProperty("HLA")]
        public float HLA { get; set; }
        [JsonProperty("VLA")]
        public float VLA { get; set; }
        [JsonProperty("CarryDistance")]
        public float? CarryDistance { get; set; }
    }
}
