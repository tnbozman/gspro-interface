using GSProInterface.Extensions;
using GSProInterface.Models.enums;
using GSProInterface.Models.Reponse;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Services
{
    public static class ValidateResponse 
    {
        public static bool IsValid(ResponseDto response, ILogger<IGSProInterface> logger)
        {
            if (response == null || response.Code >= 500)
            {
                if(response == null)
                {
                    logger.LogDebug("Response not found (null).");
                }
                else
                {
                    logger.LogDebug($"An error has be detected GSPro has responded with Code: {response.Code} - {ResponseCodeDescription(response.Code)}.");
                }
                return false;
            }
            logger.LogDebug($"GSPro has responded with Code: {response.Code} - {ResponseCodeDescription(response.Code)}.");
            return true;
        }

        public static bool IsValid(ResponseDto response, int expectedCode, ILogger<IGSProInterface> logger)
        {
            if(!IsValid(response, logger))
                return false;

            if(response.Code == expectedCode)
            {
                logger.LogDebug($"GSPro has responded with Code: {response.Code} - {ResponseCodeDescription(response.Code)}.");
                return true;
            }

            logger.LogDebug($"GSPro has responded with an unexpected Code: {response.Code} - {ResponseCodeDescription(response.Code)}.");
            return false;
            
        }

        private static string ResponseCodeDescription(int code)
        {
            switch (code)
            {
                case (int)ResponseCodes.FAILURE:
                    return ResponseCodes.FAILURE.GetDescription();
                case (int)ResponseCodes.PLAYER_INFO:
                    return ResponseCodes.PLAYER_INFO.GetDescription();
                case (int)ResponseCodes.SHOT_SUCCESS:
                    return ResponseCodes.SHOT_SUCCESS.GetDescription();
                default:
                    return "Code Description Not Found";
            }
        }
    }
}
