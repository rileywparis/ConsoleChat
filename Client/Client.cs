using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Client
    {
        private static readonly Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const int PORT = 25565;
        static string name = "user";

        static void Main(string[] args)
        {
            Console.Title = "Client";
            Console.SetWindowSize(80, 5);
            while (!ClientSocket.Connected)
                try
                {
                    Console.WriteLine("Trying to connect");
                    ClientSocket.Connect("172.20.8.252", PORT);
                }
                catch (SocketException)
                {
                    Console.Clear();
                }
            Console.Clear();
            Console.WriteLine("Connected");
            Console.WriteLine("Enter username: ");
            name = Console.ReadLine();
            Console.Clear();
            Process.Start(@"C:\Users\panda\source\repos\ConsoleChat\ChatWindow\bin\Debug\ChatWindow.exe");

            while (true)
                SendMessage();
        }

        private static void SendMessage()
        {
            Console.Write("Type: ");
            string message = Console.ReadLine();
            byte[] buffer = Encoding.ASCII.GetBytes(name + ": " + message);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            Console.Clear();
        }
    }
}
