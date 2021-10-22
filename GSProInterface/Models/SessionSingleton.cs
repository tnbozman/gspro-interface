using GSProInterface.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Models
{
    public sealed class SessionSingleton
    {
        SessionSingleton() {
            ShotNumber = 1;
            Setup(IPAddress.Parse("127.0.0.1"), 0921);
            SetupSession("tbd", Units.METERS, APIVersion._1);
        }

        private static SessionSingleton instance = null;
        public static SessionSingleton Instance()
        {
            if (instance == null)
            {
                instance = new SessionSingleton();
            }
            return instance;
        }

        public static void Setup(IPAddress ipAddress, int port)
        {
            IpAddress = ipAddress;
            Port = port;
        }

        public static void SetupSession(string deviceId, Units units, APIVersion apiVersion)
        {
            DeviceID = deviceId;
            Units = units;
            APIVersion = apiVersion;
        }


        public static void ShotTaken()
        {
            ShotNumber++;
        }
        
        public static string DeviceID { get; private set; }
        public static Units Units { get; private set; }
        public static int ShotNumber { get; private set; }
        public static APIVersion APIVersion { get; private set; }
        public static int Port { get; private set; }
        public static IPAddress IpAddress { get; private set; }


    }
}
