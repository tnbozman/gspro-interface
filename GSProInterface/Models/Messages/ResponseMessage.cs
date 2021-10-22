using GSProInterface.Models.Reponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models.Messages
{
    public class ResponseMessage: MessageBase
    {
        public ResponseMessage(ResponseDto payload)
        {
            HasError = false;
            Payload = payload;
        }

        public ResponseDto Payload { get; set; }
    }
}
