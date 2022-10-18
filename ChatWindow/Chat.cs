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
        private static readonly Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            Console.WriteLine(Encoding.ASCII.GetString(data));
        }
    }
}
