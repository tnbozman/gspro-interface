using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarminInterface
{
    interface GarminSocketInterface
    {
        void Connect();
        void Disconnect();
        void SendHandshake();
        void SendPing();
        void SendShot();


    }
}
