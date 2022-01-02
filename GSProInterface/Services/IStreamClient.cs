using GSProInterface.Models.enums;
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
        Status Status { get; }

        event Action<IStreamClient> ClientConnected;
        event Action<IStreamClient> ClientDisconnected;
        event Action<IStreamClient, ResponseDto> ShotReceived;
        event Action<IStreamClient, ResponseDto> PlayerInformationReceived;
        event Action<IStreamClient, ResponseDto> GSProReadyReceived;
        event Action<IStreamClient, ResponseDto> EndOfRoundReceived;
        event Action<IStreamClient, string> ErrorDetected;

        void Connect(string address, int port);
        void Disconnect();
        void Send(ShotDataDto data);
        ResponseDto Receive();
    }
}
