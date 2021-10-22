using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class GSProException : Exception
{
    public GSProException()
    {
    }

    public GSProException(string message)
        : base(message)
    {
    }

    public GSProException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
