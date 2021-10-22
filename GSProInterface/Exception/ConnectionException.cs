using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ConnectionException: GSProException
{
    public ConnectionException()
    {
    }

    public ConnectionException(string message)
        : base(message)
    {
    }

    public ConnectionException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
