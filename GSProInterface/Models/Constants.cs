using GSProInterface.Extensions;
using GSProInterface.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models
{
    public class Constants
    {
        public const Units UNITS = Units.YARDS;
        public const APIVersion API_VERSION = APIVersion._1;
        public const int ReceiveBuffer = 2048;
        public const int SendBuffer = 2048;

    }
}
