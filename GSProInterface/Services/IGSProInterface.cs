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
        void StartClient();
        void StopClient();
        void SendLaunchMonitorStatus(bool LaunchMonitorIsReady);
        ResponseDto SendBallData(BallDataDto ballData);
        ResponseDto SendClubData(ClubDataDto shotData);
        ResponseDto SendBallAndClubData(BallDataDto ballData, ClubDataDto shotData);
    }
}
