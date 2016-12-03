using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPSServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPAddress ip = Dns.GetHostByName(host).AddressList[0];
            Console.WriteLine("IpAdress " + IPAddress.Loopback);

            Connection con = new Connection(11000, Console.Out);
            con.Work();
        }
    }
}
