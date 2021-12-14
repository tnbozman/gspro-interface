using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models.enums
{
    public enum PlayerClubs
    {
        [Description("DR")]
        DRIVER = 1,
        [Description("W2")]
        TWO_WOOD = 2,
        [Description("W3")]
        THREE_WOOD = 3,
        [Description("W4")]
        FOUR_WOOD = 4,
        [Description("W5")]
        FIVE_WOOD = 5,
        [Description("W6")]
        SIX_WOOD = 6,
        [Description("W7")]
        SEVEN_WOOD = 7,
        [Description("H2")]
        TWO_HYBRID = 8,
        [Description("H3")]
        THREE_HYBRID = 9,
        [Description("H4")]
        FOUR_HYBRID = 10,
        [Description("H5")]
        FIVE_HYBRID = 11,
        [Description("H6")]
        SIX_HYBRID = 12,
        [Description("H7")]
        SEVEN_HYBRID = 13,
        [Description("I1")]
        ONE_IRON = 14,
        [Description("I2")]
        TWO_IRON = 15,
        [Description("I3")]
        THREE_IRON = 16,
        [Description("I4")]
        FOUR_IRON = 17,
        [Description("I5")]
        FIVE_IRON = 18,
        [Description("I6")]
        SIX_IRON = 19,
        [Description("I7")]
        SEVEN_IRON = 20,
        [Description("I8")]
        EIGHT_IRON = 21,
        [Description("I9")]
        NINE_IRON = 22,
        [Description("PW")]
        PITCHING_WEDGE = 23,
        [Description("GW")]
        GAP_WEDGE = 24,
        [Description("SW")]
        SAND_WEDGE = 25,
        [Description("LW")]
        LOB_WEDGE = 26,
        [Description("PT")]
        PUTTER = 27,
    }
}
