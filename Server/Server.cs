using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;

namespace Server
{
    public class Server : WatsonTcpServer
    {
        public Server(string listenerIp, int listenerPort) : base(listenerIp, listenerPort)
        {
        }
    }
}
