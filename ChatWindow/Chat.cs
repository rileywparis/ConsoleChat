using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatWindow
{
    internal class Chat
    {
        private static Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const int PORT = 25565;

        static void Main(string[] args)
        {
            Console.Title = "Chat Window";
            ClientSocket.Connect("172.20.8.252", PORT);
            Console.Clear();
            while (true)
                ReceiveMessage();
        }

        private static void ReceiveMessage()
        {
            var buffer = new byte[2048];
            ClientSocket.Receive(buffer);
            Console.WriteLine(Encoding.ASCII.GetString(buffer));
        }
    }
}
