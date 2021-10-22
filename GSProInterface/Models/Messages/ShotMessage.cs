using GSProInterface.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models.Messages
{
    public class ShotMessage: MessageBase
    {
        public ShotMessage(ShotDataDto payload)
        {
            Payload = payload;
        }

        public ShotDataDto Payload { get; set; }
    }
}
