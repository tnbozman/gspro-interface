using GSProInterface.Adapters;
using GSProInterface.Models;
using GSProInterface.Models.enums;
using GSProInterface.Models.Reponse;
using GSProInterface.Models.Request;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GSProInterface.Services
{
    public class GSProStreamInterface: IGSProInterface
    {
        // fulfilled by dependency injection
        private readonly IStreamClient _client;
        private readonly ILogger<IGSProInterface> _logger;
        private readonly IDeviceDetails _deviceDetails;

        // Socket Client Status exposed via the interface
        public Status Status => _client.Status;

        #region Events
        // signals the client has successfully established a socket connection to GSPro Connect
        public event Action<IGSProInterface> ClientConnected;
        // signals that the socket to GSPro Connect has been disconnected
        public event Action<IGSProInterface> ClientDisconnected;
        // signals that a response to a shot has been received from GSPro Connect
        public event Action<IGSProInterface, ResponseDto> ShotReceived;
        public event Action<IGSProInterface, ResponseDto> GSProReadyReceived;
        public event Action<IGSProInterface, ResponseDto> EndOfRoundReceived;
        // signals that a response to play information change in GSPro has been received from GSPro Connect
        public event Action<IGSProInterface, ResponseDto> PlayerInformationReceived;
        // An error has occured
        public event Action<IGSProInterface, string> ErrorDetected;
        #endregion

        public GSProStreamInterface(ILogger<IGSProInterface> logger, IStreamClient client, IDeviceDetails deviceDetails)
        {
            _logger = logger; 
            _client = client;
            _deviceDetails = deviceDetails;

            // Register onto the socket clients events to propagate them to the events of this interface
            _client.ClientConnected += this.OnClientConnected;
            _client.ClientDisconnected += this.OnClientDisconnected;
            _client.ShotReceived += this.OnShotReceived;
            _client.PlayerInformationReceived += this.OnPlayerInformationReceived;
            _client.ErrorDetected += this.OnErrorDetected;
            _client.GSProReadyReceived += this.OnGSProReady;
            _client.EndOfRoundReceived += this.OnEndOfRound;
        }


        #region Methods
        /// <summary>
        /// Attempt to make a connection to GSPro Connect using the provided ip address and port
        /// Successful connection will raise the ClientConnected event
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public void StartClient(string address, int port)
        {
            _logger.LogDebug($"Attempting to connect to {address}:{port}");
            _client.Connect(address, port);                    
        }

        /// <summary>
        /// Successful disconnection will raise the ClientDisconnected event
        /// </summary>
        public void StopClient()
        {
            _logger.LogDebug($"Stopping client connection.");
            _client.Disconnect();

        }

        /// <summary>
        /// Send a shot to GSPro Connect that contains ball and club data and retrieve the shot's GSPro response
        /// </summary>
        /// <param name="ballData"></param>
        /// <param name="clubData"></param>
        /// <returns></returns>
        public ResponseDto SendBallAndClubData(BallDataDto ballData, ClubDataDto clubData)
        {
            _logger.LogDebug($"Sending ball and club data.");
            var shotData = ShotAdapter.ShotWithClubAndBallDataToShot(clubData, ballData, _deviceDetails);
            return ValidateShotResponse(SendShotData(shotData));
        }

        /// <summary>
        /// Send a shot to GSPro Connect that contains ball data and retrieve the shot's GSPro response
        /// </summary>
        /// <param name="ballData"></param>
        /// <returns></returns>
        public ResponseDto SendBallData(BallDataDto ballData)
        {
            _logger.LogDebug($"Sending ball data only.");
            var shotData = ShotAdapter.ShotWithBallDataToShot(ballData, _deviceDetails);
            return ValidateShotResponse(SendShotData(shotData));
        }

        /// <summary>
        /// Send a shot to GSPro Connect that contains club data and retrieve the shot's GSPro response
        /// </summary>
        /// <param name="clubData"></param>
        /// <returns></returns>
        public ResponseDto SendClubData(ClubDataDto clubData)
        {
            _logger.LogDebug($"Sending club data only.");
            var shotData = ShotAdapter.ShotWithClubDataToShot(clubData, _deviceDetails);
            return ValidateShotResponse(SendShotData(shotData));
        }

        /// <summary>
        /// Send a launch monitor status update to GSPro Connect
        /// </summary>
        /// <param name="launchMonitorIsReady"></param>
        public void SendLaunchMonitorStatus(bool launchMonitorIsReady)
        {
            _logger.LogDebug($"Sending launch monitor status.");
            var shotData = ShotAdapter.LaunchMonitorStatusToShot(launchMonitorIsReady, _deviceDetails);
            _client.Send(shotData);
        }

        public void SendHeartBeat()
        {
            _logger.LogDebug($"Sending heatbeat.");
            var shotData = ShotAdapter.HeartBeatToShot(_deviceDetails);
            _client.Send(shotData);
        }

        /// <summary>
        /// Send the shot data object to GSPro Connect and retrieve the shot's GSPro response
        /// This method is used by all public shot methods
        /// </summary>
        /// <param name="shotData"></param>
        /// <returns></returns>
        protected ResponseDto SendShotData(ShotDataDto shotData)
        {
            _client.Send(shotData);
            return _client.Receive();
        }

        /// <summary>
        /// Validate the shot response from GSPro
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private ResponseDto ValidateShotResponse(ResponseDto response)
        {

            if (ValidateResponse.IsValid(response, (int)ResponseCodes.SHOT_SUCCESS, _logger))
            {
                _logger.LogDebug($"Shot response was successful.");
                return response;
            }

            _logger.LogDebug($"Shot response was invalid.");

            if (response != null)
            {
                return new ResponseDto
                {
                    Code = (int)ResponseCodes.FAILURE,
                    Message = "Shot response was null."
                };
                
            }
            return response;
        }
        #endregion

        #region Socket Client Event Handlers

        /// <summary>
        /// Handle the connect event from the socket client by emitting the event within this interface
        /// </summary>
        /// <param name="client"></param>
        protected virtual void OnClientConnected(IStreamClient client)
        {
            if (ClientConnected != null) ClientConnected.Invoke(this);
        }

        /// <summary>
        /// Handle the disconnect event from the socket client by emitting the event within this interface
        /// </summary>
        /// <param name="client"></param>
        protected virtual void OnClientDisconnected(IStreamClient client)
        {
            if (ClientDisconnected != null) ClientDisconnected.Invoke(this);
        }

        /// <summary>
        /// Handle the shot received event from the socket client by emitting the event within this interface
        /// </summary>
        /// <param name="client"></param>
        /// <param name="response"></param>
        protected virtual void OnShotReceived(IStreamClient client, ResponseDto response)
        {
            if (ShotReceived != null) ShotReceived.Invoke(this, response);
        }

        /// <summary>
        /// Handle the play information received event from the socket client by emitting the event within this interface
        /// </summary>
        /// <param name="client"></param>
        /// <param name="response"></param>
        protected virtual void OnPlayerInformationReceived(IStreamClient client, ResponseDto response)
        {
            if (PlayerInformationReceived != null) PlayerInformationReceived.Invoke(this, response);
        }


        protected virtual void OnGSProReady(IStreamClient client, ResponseDto response)
        {
            if (GSProReadyReceived != null) GSProReadyReceived.Invoke(this, response);
        }
        protected virtual void OnEndOfRound(IStreamClient client, ResponseDto response)
        {
            if (EndOfRoundReceived != null) EndOfRoundReceived.Invoke(this, response);

        }

        /// <summary>
        /// Handle the error event from the socket client by emitting the event within this interface
        /// </summary>
        /// <param name="client"></param>
        /// <param name="response"></param>
        protected virtual void OnErrorDetected(IStreamClient client, string response)
        {
            if (ErrorDetected != null) ErrorDetected.Invoke(this, response);
        }

        #endregion

    }
}
