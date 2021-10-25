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
    public interface IGSProInterface
    {

        Status Status { get; }

        event Action<IGSProInterface> ClientConnected;
        event Action<IGSProInterface> ClientDisconnected;
        event Action<IGSProInterface, ResponseDto> ShotReceived;
        event Action<IGSProInterface, ResponseDto> PlayerInformationReceived;
        event Action<IGSProInterface, string> ErrorDetected;

        void StartClient(string address, int port);
        void StopClient();
        void SendLaunchMonitorStatus(bool LaunchMonitorIsReady);
        ResponseDto SendBallData(BallDataDto ballData);
        ResponseDto SendClubData(ClubDataDto shotData);
        ResponseDto SendBallAndClubData(BallDataDto ballData, ClubDataDto shotData);
    }
}
