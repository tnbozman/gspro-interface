using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarminInterface.Models.enums
{
    public enum ResponseTypes
    {
        [Description("ACK")]
        ACK = 1,
        [Description("SimCommand")]
        SIM_COMMAND = 2
    }
}
