using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarminInterface.Models.enums
{
    public enum RequestTypes
    {
        [Description("Handshake")]
        HANDSHAKE = 1,
        [Description("SetClubType")]
        SET_CLUB_TYPE = 2,
        [Description("SetBallData")]
        SET_BALL_DATA = 3,
        [Description("SetClubData")]
        SET_CLUB_DATA = 4,
        [Description("SendShot")]
        SEND_SHOT = 5
    }
}
