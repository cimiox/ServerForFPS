using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FPSServer
{
    class ClientInfo
    {
        public TcpClient Client { get; set; }
        public bool IsConnect { get; set; }
        public List<byte> buffer = new List<byte>();

        public ClientInfo(TcpClient Client)
        {
            this.Client = Client;
            IsConnect = true;
        }
    }
}
