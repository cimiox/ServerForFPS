using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CommandsLibrary
{
    public class Client
    {
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }
        public float RotW { get; set; }

        public float Health { get; set; }
        public string Nickname { get; set; }

        public bool Team { get; set; }

        public Client(TcpClient client)
        {
            string Request = "";
            byte[] Buffer = new byte[1024];
            int Count;
            while ((Count = client.GetStream().Read(Buffer, 0, Buffer.Length)) > 0)
            {
                Request += Encoding.ASCII.GetString(Buffer, 0, Count);

                if (Request.IndexOf("\r\n\r\n") >= 0 || Request.Length > 4096)
                {
                    break;
                }

                client.Close();
            }
        }
    }
}
