using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ShotException: GSProException
{
    public ShotException()
    {
    }

    public ShotException(string message)
        : base(message)
    {
    }

    public ShotException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

