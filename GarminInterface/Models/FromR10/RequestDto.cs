using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarminInterface.Models.FromR10
{
    // the data transfer object from the r10
    public class RequestDto
    {
        [JsonProperty("Type")]
        public string Type { get; set; }
        #region club properties
        [JsonProperty("ClubType")]
        public string? ClubType {get; set;}
        #endregion
        #region challenge properties
        [JsonProperty("Developer")]
        public string? Developer { get; set; }
        [JsonProperty("Hash")]
        public string? Hash { get; set; }
        #endregion
    }
}
