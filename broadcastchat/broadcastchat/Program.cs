using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace broadcastchat
{
    class Program
    {
        private const int ListenPort = 11000;

        static void Main(string[] args)
        {
            var listenThread = new Thread(Listener);
            listenThread.Start();

            Socket socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
            socket.EnableBroadcast = true;
            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 11000);

            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.BackgroundColor = ConsoleColor.White;
            while (true)
            {
                Console.Write(">");
                string msg = Console.ReadLine();

                byte[] sendbuf = Encoding.UTF8.GetBytes(msg);
                socket.SendTo(sendbuf, ep);
                Thread.Sleep(200);
            }
  
        }

        static void Listener()
        {
            UdpClient listener = new UdpClient(ListenPort);

            try
            {
                while (true)
                {
                    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, ListenPort);
                    DateTime time = DateTime.Now;
                    byte[] bytes = listener.Receive(ref groupEP);
                    Console.WriteLine("Received broadcast from {0} : {1} {2}\n", groupEP.ToString(), time, Encoding.UTF8.GetString(bytes, 0, bytes.Length));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }
        }
    }
}
