using System;
using System.Threading;

namespace FPSServer
{
    class Program
    {
        static Connection server; // сервер
        static Thread listenThread; // потока для прослушивания

        static void Main(string[] args)
        {
            try
            {
                server = new Connection();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
