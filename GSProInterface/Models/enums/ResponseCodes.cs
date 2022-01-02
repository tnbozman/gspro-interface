using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models.enums
{
    public enum ResponseCodes
    {
        [Description("Shot received successfully")]
        SHOT_SUCCESS = 200,
        [Description("Player information")]
        PLAYER_INFO = 201,
        [Description("GSPro Ready")]
        GSPRO_READY = 202,
        [Description("Round Ended")]
        ROUND_ENDED = 203,
        [Description("Failure occurred")]
        FAILURE = 501 
    }
}
