using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models.Messages
{
    public class MessageBase
    {
        public bool HasError { get; set; }
        public Exception Exception { get; set; }

        public MessageBase()
        {
            Exception = new Exception();
        }
    }
}
