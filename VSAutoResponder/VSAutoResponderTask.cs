using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSAutoResponder.Msbuild
{
    using Fiddler.VSAutoResponder;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization.Json;
    public class VSAutoResponderClient
    {
        // TODO When executed it simply loads the project file. Parses it for all files + properties
        //      Packages it all into a nice zeromq message that is then sent to the fiddler process. 
        //      If the fiddler process is online it will recieve the messages and translate it into rules.
                
        public VSAutoResponderClient()
        {

        }

        public void Add(params AutoResponderRule[] rules)
        {
            // Create a udp client. Serialize messages and send them
            using (var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                var serializer = new DataContractJsonSerializer(typeof(AutoResponderRule[]));
                using (var mS = new MemoryStream())
                {
                    serializer.WriteObject(mS, rules);
                    udpSocket.SendTo(mS.GetBuffer(), (int)mS.Length, SocketFlags.None, Configuration.EndPoint);
                }                        
            }            
        }
    }
}
