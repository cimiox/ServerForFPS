using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPSServer
{
    class Connection
    {
        static Connection Instance;

        public TcpListener Listener;
        public List<ClientInfo> clients = new List<ClientInfo>();
        public List<ClientInfo> NewClients = new List<ClientInfo>();
        public TextWriter Out;

        public Connection(int Port, TextWriter _out)
        {
            Out = _out;
            Instance = this;
            Listener = new TcpListener(IPAddress.Loopback, Port);
            Listener.Start();
        }

        ~Connection()
        {
            if (Listener != null)
            {
                Listener.Stop();
            }
        }

        public void Work()
        {
            Thread clientListener = new Thread(ListenerClients);
            clientListener.Start();

            while (true)
            {
                foreach (ClientInfo item in clients)
                {
                    if (item.IsConnect)
                    {
                        NetworkStream stream = item.Client.GetStream();
                        while (stream.DataAvailable)
                        {

                            int readByte = stream.ReadByte();
                            if (readByte != -1)
                            {
                                item.buffer.Add((byte)readByte);
                            }
                        }

                        if (item.buffer.Count > 0)
                        {
                            foreach (ClientInfo otherClients in clients)
                            {
                                byte[] msg = item.buffer.ToArray();
                                Console.WriteLine(Encoding.UTF8.GetString(msg));
                                
                                item.buffer.Clear();

                                foreach (ClientInfo otherOtherClients in clients)
                                {
                                    if (otherOtherClients != item)
                                    {
                                        try
                                        {
                                            otherOtherClients.Client.GetStream().Write(msg, 0, msg.Length);

                                            byte[] byt = new byte[1024];
                                            otherOtherClients.Client.GetStream().Read(byt, 0, byt.Length);
                                            //Console.WriteLine(Encoding.UTF8.GetString(byt));
                                        }
                                        catch (Exception)
                                        {
                                            otherOtherClients.IsConnect = false;
                                            otherOtherClients.Client.Close();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                clients.RemoveAll(delegate (ClientInfo clientInfo)
                {
                    if (!clientInfo.IsConnect)
                    {
                        Instance.Out.WriteLine("Client disconnect");
                        return true;
                    }

                    return false;
                });

                if (NewClients.Count > 0)
                {
                    clients.AddRange(NewClients);
                    NewClients.Clear();
                }
            }
        }

        static void ListenerClients()
        {
            while (true)
            {
                Instance.NewClients.Add(new ClientInfo(Instance.Listener.AcceptTcpClient()));
                Instance.Out.WriteLine("New Client");
            }
        }
    }
}
