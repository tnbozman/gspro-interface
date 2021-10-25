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
        private readonly IStreamClient _client;

        private readonly ILogger<IGSProInterface> _logger;

        public Status Status => _client.Status;

        public event Action<IGSProInterface> ClientConnected;
        public event Action<IGSProInterface> ClientDisconnected;
        public event Action<IGSProInterface, ResponseDto> ShotReceived;
        public event Action<IGSProInterface, ResponseDto> PlayerInformationReceived;
        public event Action<IGSProInterface, string> ErrorDetected;

        public GSProStreamInterface(ILogger<IGSProInterface> logger, IStreamClient client)
        {
            _logger = logger; 
            _client = client;

            _client.ClientConnected += this.OnClientConnected;
            _client.ClientDisconnected += this.OnClientDisconnected;
            _client.ShotReceived += this.OnShotReceived;
            _client.PlayerInformationReceived += this.OnPlayerInformationReceived;
            _client.ErrorDetected += this.OnErrorDetected;
        }

        public void StartClient(string address, int port)
        {
            _logger.LogDebug($"Attempting to connect to {address}:{port}");
            _client.Connect(address, port);                    
        }

        public void StopClient()
        {
            _logger.LogDebug($"Stopping client connection.");
            _client.Disconnect();

        }

        public ResponseDto SendBallAndClubData(BallDataDto ballData, ClubDataDto clubData)
        {
            _logger.LogDebug($"Sending ball and club data.");
            var shotData = ShotAdapter.ShotWithClubAndBallDataToShot(clubData, ballData);
            return ValidateShotResponse(SendShotData(shotData));
        }

        public ResponseDto SendBallData(BallDataDto ballData)
        {
            _logger.LogDebug($"Sending ball data only.");
            var shotData = ShotAdapter.ShotWithBallDataToShot(ballData);
            return ValidateShotResponse(SendShotData(shotData));
        }

        public ResponseDto SendClubData(ClubDataDto clubData)
        {
            _logger.LogDebug($"Sending club data only.");
            var shotData = ShotAdapter.ShotWithClubDataToShot(clubData);
            return ValidateShotResponse(SendShotData(shotData));
        }

        public void SendLaunchMonitorStatus(bool launchMonitorIsReady)
        {
            _logger.LogDebug($"Sending launch monitor status.");
            var shotData = ShotAdapter.LaunchMonitorStatusToShot(launchMonitorIsReady);
            _client.Send(shotData);
        }

        protected ResponseDto SendShotData(ShotDataDto shotData)
        {
            _client.Send(shotData);
            return _client.Receive();
        }

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

        protected virtual void OnClientConnected(IStreamClient client)
        {
            if (ClientConnected != null) ClientConnected.Invoke(this);
        }

        protected virtual void OnClientDisconnected(IStreamClient client)
        {
            if (ClientDisconnected != null) ClientDisconnected.Invoke(this);
        }

        protected virtual void OnShotReceived(IStreamClient client, ResponseDto response)
        {
            if (ShotReceived != null) ShotReceived.Invoke(this, response);
        }

        protected virtual void OnPlayerInformationReceived(IStreamClient client, ResponseDto response)
        {
            if (PlayerInformationReceived != null) PlayerInformationReceived.Invoke(this, response);
        }

        protected virtual void OnErrorDetected(IStreamClient client, string response)
        {
            if (ErrorDetected != null) ErrorDetected.Invoke(this, response);
        }

    }
}
