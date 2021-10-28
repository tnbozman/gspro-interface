using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models.Request
{
    public class ShotDataDto
    {
        [JsonProperty("DeviceID")]
        public string DeviceID { get; set; }
        [JsonProperty("Units")]
        public string Units { get; set; }
        [JsonProperty("ShotNumber")]
        public int ShotNumber { get; set; }
        [JsonProperty("APIversion")]
        public string APIVersion { get; set; }
        [JsonProperty("BallData")]
        public BallDataDto BallData { get; set; }
        [JsonProperty("ClubData")]
        public ClubDataDto? ClubData { get; set; }
        [JsonProperty("ShotDataOptions")]
        public ShotDataOptionsDto? ShotDataOptions { get; set; }
    }
}
