using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models.Reponse
{
    public class ResponseDto
    {
        [JsonProperty("Code")]
        public int Code { get; set; }
        [JsonProperty("Message")]
        public String Message {get;set;}
        [JsonProperty("Player")]
        public PlayerDto Player { get; set; }
    }
}
