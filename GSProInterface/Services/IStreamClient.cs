using GSProInterface.Models.Reponse;
using GSProInterface.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Services
{
    public interface IStreamClient
    {
        void Connect(string address, int port);
        void Disconnect();
        void Send(ShotDataDto data);
        ResponseDto Receive();
    }
}
