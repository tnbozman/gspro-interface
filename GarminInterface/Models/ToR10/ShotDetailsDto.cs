using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarminInterface.Models.ToR10
{
    public class ShotDetailsDto
    {
        [JsonProperty("Apex")]
        public double Apex { get; set; }
        [JsonProperty("BallData")]
        public BallDataDto BallData { get; set; }
        [JsonProperty("BallInHole")]
        public bool BallInHole { get; set; }
        [JsonProperty("BallLocation")]
        public string BallLocation { get; set; } // Fringe, 
        [JsonProperty("CarryDeviationAngle")]
        public double CarryDeviationAngle { get; set; }
        [JsonProperty("CarryDeviationFeet")]
        public double CarryDeviationFeet { get; set; }
        [JsonProperty("CarryDistance")]
        public double CarryDistance { get; set; }
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
