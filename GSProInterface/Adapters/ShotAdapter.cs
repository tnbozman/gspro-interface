using GSProInterface.Extensions;
using GSProInterface.Models;
using GSProInterface.Models.Request;
using GSProInterface.Services;
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

        public static BallDataDto WinformToBallData(decimal speed, decimal spinAxis, decimal totalSpin, decimal hla, decimal vla)
        {
            return new BallDataDto
            {
                Speed = DecimalToFloat(speed),
                SpinAxis = DecimalToFloat(spinAxis),
                TotalSpin = DecimalToFloat(totalSpin),
                SideSpin = null,
                BackSpin = null,
                CarryDistance =  null,
                HLA = DecimalToFloat(hla),
                VLA = DecimalToFloat(vla)
            };
        }

        public static ClubDataDto WinformToClubData(decimal speed, decimal aoa, decimal ftt, decimal lie, decimal loft, decimal path, decimal speedAtImpact, decimal vfi, decimal hfi, decimal closureRate)
        {
            return new ClubDataDto
            {
                Speed = DecimalToFloat(speed),
                AngleOfAttack = DecimalToFloat(aoa),
                FaceToTarget = DecimalToFloat(ftt),
                Lie = DecimalToFloat(lie),
                Loft = DecimalToFloat(loft),
                Path = DecimalToFloat(path),
                SpeedAtImpact = DecimalToFloat(speedAtImpact),
                VerticalFaceImpact = DecimalToFloat(vfi),
                HorizontalFaceImpact = DecimalToFloat(hfi),
                ClosureRate = DecimalToFloat(closureRate)
            };
        }

        private static float DecimalToFloat(decimal value)
        {
            return (float)value;
        }
        public static ShotDataDto ShotWithBallDataToShot(BallDataDto ballData, IDeviceDetails deviceDetails)
        {
            return PrepareShotData(ballData, null, ShotOptionsForShot(true, false), deviceDetails);
        }

        public static ShotDataDto ShotWithClubDataToShot(ClubDataDto clubData, IDeviceDetails deviceDetails)
        {
            return PrepareShotData(null, clubData, ShotOptionsForShot(false, true), deviceDetails);
        }

        public static ShotDataDto ShotWithClubAndBallDataToShot(ClubDataDto clubData, BallDataDto ballData, IDeviceDetails deviceDetails)
        {
            return PrepareShotData(ballData, clubData, ShotOptionsForShot(true, true), deviceDetails);
        }

        public static ShotDataDto LaunchMonitorStatusToShot(bool launchMonitorIsReady, IDeviceDetails deviceDetails)
        {
            return PrepareShotData(null, null, ShotOptionsForLaunchMonitorStatus(launchMonitorIsReady), deviceDetails);
        }

        public static ShotDataDto HeartBeatToShot(IDeviceDetails deviceDetails)
        {
            return PrepareShotData(null, null, ShotOptionsForHearBeat(), deviceDetails);
        }


        private static ShotDataDto PrepareShotData(BallDataDto ballData, ClubDataDto clubData, ShotDataOptionsDto options, IDeviceDetails deviceDetails)
        {
            ShotNumber++;
            return new ShotDataDto
            {
                DeviceID = deviceDetails.DeviceName,
                Units = Constants.UNITS.GetDescription(),
                ShotNumber = ShotNumber,
                APIVersion = Constants.API_VERSION.GetDescription(),
                ClubData = clubData,
                BallData = ballData,
                ShotDataOptions = options
            };

        }

        private static ShotDataOptionsDto ShotOptionsForHearBeat()
        {
            return new ShotDataOptionsDto
            {
                ContainsBallData = false,
                ContainsClubData = false,
                LaunchMonitorBallDetected = false,
                LaunchMonitorIsReady = false,
                IsHeartBeat = true
            };
        }


        private static ShotDataOptionsDto ShotOptionsForLaunchMonitorStatus(bool launchMonitorIsReady)
        {
            return new ShotDataOptionsDto
            {
                ContainsBallData = false,
                ContainsClubData = false,
                LaunchMonitorBallDetected = false,
                LaunchMonitorIsReady = launchMonitorIsReady,
                IsHeartBeat = false
            };
        }

        private static ShotDataOptionsDto ShotOptionsForShot(bool ballData, bool shotData)
        {
            return new ShotDataOptionsDto
            {
                ContainsBallData = ballData,
                ContainsClubData = shotData,
                LaunchMonitorBallDetected = true,
                LaunchMonitorIsReady = true,
                IsHeartBeat = false
            };
        }
    }
}
