using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Client
    {
        private const int PORT = 25565;
        private static string PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\AppServer\Upload";

        static void Main(string[] args)
        {
            Directory.CreateDirectory(PATH);
            Console.Title = "Client";
            Console.SetWindowSize(80, 5);
            //while (!clientSocket.Connected)
            //    try
            //    {
            //        Console.WriteLine("Trying to connect");
            //        clientSocket.Connect("172.20.8.252", PORT);
            //    }
            //    catch (SocketException)
            //    {
            //        Console.Clear();
            //    }
            Console.Clear();
            Console.WriteLine("Connected");

            while (true)
                SendMessage();
        }

        private static void SendMessage()
        {
            Console.Write("Type: ");
            string message = Console.ReadLine();
            if (message == "send")
            {
                foreach (string f in Directory.GetFiles(PATH))
                {
                    Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    byte[] fileNameByte = Encoding.ASCII.GetBytes(f);
                    byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
                    byte[] fileData = File.ReadAllBytes(f);
                    byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];

                    fileNameLen.CopyTo(clientData, 0);
                    fileNameByte.CopyTo(clientData, 4);
                    fileData.CopyTo(clientData, 4 + fileNameByte.Length);
                    clientSocket.Connect("172.20.8.252", PORT);
                    clientSocket.Send(clientData);
                    clientSocket.Close();
                }
            }
            Console.Clear();
        }
    }
}
