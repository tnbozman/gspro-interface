using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models.Messages
{
    public class ErrorMessage: MessageBase
    {
        public ErrorMessage(Exception ex)
        {
            HasError = true;
            Exception = ex;
        }
    }
}
