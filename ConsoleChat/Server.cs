using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleChat
{
    internal class Server
    {
        private static List<Socket> clientSockets = new List<Socket>();
        private const int PORT = 25565;
        private static string PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\AppServer";

        static void Main(string[] args)
        {
            Directory.CreateDirectory(PATH);
            Console.Title = "Server";
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("172.20.8.252"), PORT));
            serverSocket.Listen(PORT);
            Console.WriteLine("Server Started");

            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                new Thread(delegate ()
                {
                    Receive(clientSocket);
                }).Start();
            }
        }

        public static void Receive(Socket clientSocket)
        {
            byte[] clientData = new byte[1024 * 5000];
            int receivedBytesLen = clientSocket.Receive(clientData);
            int fileNameLen = BitConverter.ToInt32(clientData, 0);
            string fileName = new DirectoryInfo(Encoding.ASCII.GetString(clientData, 4, fileNameLen)).Name;
            Console.WriteLine(fileName);
            BinaryWriter bWrite = new BinaryWriter(File.Open(PATH + @"\" + fileName, FileMode.Create));
            bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
            bWrite.Close();
            clientSocket.Close();
        }
    }
}
