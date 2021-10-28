using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models.Request
{
    public class ClubDataDto
    {
        [JsonProperty("Speed")]
        public float Speed { get; set; }
        [JsonProperty("AngleOfAttack")]
        public float AngleOfAttack { get; set; }
        [JsonProperty("FaceToTarget")]
        public float FaceToTarget { get; set; }
        [JsonProperty("Lie")]
        public float Lie { get; set; }
        [JsonProperty("Loft")]
        public float Loft { get; set; }
        [JsonProperty("Path")]
        public float Path { get; set; }
        [JsonProperty("SpeedAtImpact")]
        public float SpeedAtImpact { get; set; }
        [JsonProperty("VerticalFaceImpact")]
        public float VerticalFaceImpact { get; set; }
        [JsonProperty("HorizontalFaceImpact")]
        public float HorizontalFaceImpact { get; set; }
        [JsonProperty("ClosureRate")]
        public float ClosureRate { get; set; }
    }
}
