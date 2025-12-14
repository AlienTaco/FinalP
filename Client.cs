using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;

namespace FinalP
{
    public class Client : WatsonTcpClient
    {
        public Client(string serverIp, int serverPort) : base(serverIp, serverPort)
        {
        }
    }
}
