using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarminInterface.Models.ToR10
{
    public class BallDataDto
    {
        [JsonProperty("BackSpin")]
        public double BackSpin { get; set; }
        [JsonProperty("BallSpeed")]
        public double BallSpeed { get; set; }
        [JsonProperty("LaunchAngle")]
        public double LaunchAngle { get; set; }
        [JsonProperty("LaunchDirection")]
        public double LaunchDirection { get; set; }
        [JsonProperty("SideSpin")]
        public double SideSpin { get; set; }
        [JsonProperty("SpinAxis")]
        public double SpinAxis { get; set; }
        [JsonProperty("TotalSpin")]
        public double TotalSpin { get; set; }
    }
}
