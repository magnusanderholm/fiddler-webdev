using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Fiddler.VSAutoResponder
{
    public class Configuration
    {
        public const int Port = 54712;
        public static readonly IPEndPoint EndPoint = new System.Net.IPEndPoint(IPAddress.Loopback, Configuration.Port);
        public static int MaxMessageSizeInBytes = 1024 * 1024 * 2;
    }
}
