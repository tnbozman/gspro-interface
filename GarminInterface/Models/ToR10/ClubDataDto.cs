using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarminInterface.Models.ToR10
{
    public class ClubDataDto
    {
        [JsonProperty("ClubAngleFace")]
        public double ClubAngleFace { get; set; }
        [JsonProperty("ClubAnglePath")]
        public double ClubAnglePath { get; set; }
        [JsonProperty("ClubHeadSpeed")]
        public double ClubHeadSpeed { get; set; }
        [JsonProperty("ClubHeadSpeedMPH")]
        public double ClubHeadSpeedMPH { get; set; }
        [JsonProperty("ClubType")]
        public double ClubType { get; set; }
        [JsonProperty("SmashFactor")]
        public double SmashFactor { get; set; }
        [JsonProperty("DistanceToPin")]
        public double DistanceToPin { get; set; }
        [JsonProperty("TotalDeviationAngle")]
        public double TotalDeviationAngle { get; set; }
        [JsonProperty("TotalDeviationFeet")]
        public double TotalDeviationFeet { get; set; }
        [JsonProperty("TotalDistance")]
        public double TotalDistance { get; set; }
    }
}
