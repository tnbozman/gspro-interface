using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public  class ResponseException: GSProException
{
    public ResponseException()
    {
    }

    public ResponseException(string message)
        : base(message)
    {
    }

    public ResponseException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

