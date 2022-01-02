using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models.Request
{
    public class ShotDataOptionsDto
    {
        [JsonProperty("ContainsBallData")]
        public bool ContainsBallData { get; set; }
        [JsonProperty("ContainsClubData")]
        public bool ContainsClubData { get; set; }
        [JsonProperty("LaunchMonitorIsReady")]
        public bool? LaunchMonitorIsReady { get; set; }
        [JsonProperty("LaunchMonitorBallDetected")]
        public bool? LaunchMonitorBallDetected { get; set; }
        [JsonProperty("IsHeartBeat")]
        public bool? IsHeartBeat { get; set; }
    }
}
