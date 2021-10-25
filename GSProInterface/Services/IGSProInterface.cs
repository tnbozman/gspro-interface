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
        /// <summary>
        /// Status of the underlying socket connection
        /// </summary>
        Status Status { get; }

        /// <summary>
        ///  signals the client has successfully established a socket connection to GSPro Connect
        /// </summary>
        event Action<IGSProInterface> ClientConnected;
        /// <summary>
        /// signals that the socket to GSPro Connect has been disconnected
        /// </summary>
        event Action<IGSProInterface> ClientDisconnected;
        /// <summary>
        /// signals that a response to a shot has been received from GSPro Connect
        /// NOTE: This event is not thread safe see details below
        /// </summary>
        event Action<IGSProInterface, ResponseDto> ShotReceived;
        /// <summary>
        /// signals that a response to play information change in GSPro has been received from GSPro Connect
        /// NOTE: This event is not thread safe see details below
        /// </summary>
        event Action<IGSProInterface, ResponseDto> PlayerInformationReceived;
        /// <summary>
        /// An error has occured
        /// NOTE: This event is not thread safe see details below
        /// </summary>
        event Action<IGSProInterface, string> ErrorDetected;

        /// <summary>
        /// Attempt to make a connection to GSPro Connect using the provided ip address and port
        /// Successful connection will raise the ClientConnected event
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        void StartClient(string address, int port);
        /// <summary>
        /// Successful disconnection will raise the ClientDisconnected event
        /// </summary>
        void StopClient();
        /// <summary>
        /// Send a launch monitor status update to GSPro Connect
        /// </summary>
        /// <param name="launchMonitorIsReady"></param>
        void SendLaunchMonitorStatus(bool LaunchMonitorIsReady);
        /// <summary>
        /// Send a shot to GSPro Connect that contains ball data and retrieve the shot's GSPro response
        /// </summary>
        /// <param name="ballData"></param>
        /// <returns></returns>
        ResponseDto SendBallData(BallDataDto ballData);
        /// <summary>
        /// Send a shot to GSPro Connect that contains club data and retrieve the shot's GSPro response
        /// </summary>
        /// <param name="clubData"></param>
        /// <returns></returns>
        ResponseDto SendClubData(ClubDataDto shotData);
        /// <summary>
        /// Send a shot to GSPro Connect that contains ball and club data and retrieve the shot's GSPro response
        /// </summary>
        /// <param name="ballData"></param>
        /// <param name="clubData"></param>
        /// <returns></returns>
        ResponseDto SendBallAndClubData(BallDataDto ballData, ClubDataDto shotData);
    }
}
