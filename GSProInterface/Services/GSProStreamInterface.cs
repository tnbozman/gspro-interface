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

        public GSProStreamInterface(ILogger<IGSProInterface> logger, IStreamClient client)
        {
            _logger = logger;
             
            _client = client;
        }

        public void StartClient()
        {
            try
            {
                _logger.LogDebug($"Attempting to connect to {Constants.IP_ADDRESS}:{Constants.PORT}");
                _client.Connect(Constants.IP_ADDRESS, Constants.PORT);                    
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to connect to client.");
                throw new ConnectionException("Failed to connect to GSPro", ex);
            }
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
            throw new ShotException("Response was invalid for a shot.");
        }

    }
}
