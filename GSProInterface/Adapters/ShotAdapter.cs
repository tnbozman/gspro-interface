using GSProInterface.Extensions;
using GSProInterface.Models;
using GSProInterface.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Adapters
{
    public static class ShotAdapter
    {
        private static int ShotNumber = 0;
        public static ShotDataDto ShotWithBallDataToShot(BallDataDto ballData)
        {
            return PrepareShotData(ballData, new ClubDataDto(), ShotOptionsForShot(true, false));
        }

        public static ShotDataDto ShotWithClubDataToShot(ClubDataDto clubData)
        {
            return PrepareShotData(new BallDataDto(), clubData, ShotOptionsForShot(false, true));
        }

        public static ShotDataDto ShotWithClubAndBallDataToShot(ClubDataDto clubData, BallDataDto ballData)
        {
            return PrepareShotData(ballData, clubData, ShotOptionsForShot(true, true));
        }

        public static ShotDataDto LaunchMonitorStatusToShot(bool launchMonitorIsReady)
        {
            return PrepareShotData(null, null, ShotOptionsForLaunchMonitorStatus(launchMonitorIsReady));
        }



        private static ShotDataDto PrepareShotData(BallDataDto ballData, ClubDataDto clubData, ShotDataOptionsDto options)
        {
            ShotNumber++;
            return new ShotDataDto
            {
                DeviceID = Constants.DEVICE_ID,
                Units = Constants.UNITS.GetDescription(),
                ShotNumber = ShotNumber,
                APIVersion = Constants.API_VERSION.GetDescription(),
                ClubData = clubData,
                BallData = ballData,
                ShotDataOptions = options
            };

            /*
            return new ShotDataDto
            {       
                DeviceID = SessionSingleton.DeviceID,
                Units = SessionSingleton.Units.GetDescription(),
                ShotNumber = SessionSingleton.ShotNumber,
                APIVersion = SessionSingleton.APIVersion.GetDescription(),
                ClubData = clubData,
                BallData = ballData,
                ShotDataOptions = options
            };
            */
        }


        private static ShotDataOptionsDto ShotOptionsForLaunchMonitorStatus(bool launchMonitorIsReady)
        {
            return new ShotDataOptionsDto
            {
                ContainsBallData = false,
                ContainsClubData = false,
                LaunchMonitorBallDetected = false,
                LaunchMonitorIsReady = launchMonitorIsReady
            };
        }

        private static ShotDataOptionsDto ShotOptionsForShot(bool ballData, bool shotData)
        {
            return new ShotDataOptionsDto
            {
                ContainsBallData = ballData,
                ContainsClubData = shotData,
                LaunchMonitorBallDetected = true,
                LaunchMonitorIsReady = true
            };
        }
    }
}
