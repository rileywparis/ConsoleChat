using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    internal class Client
    {
        private static readonly Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static string PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\AppServer\Upload";
        private static string PATH0 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Downloads";
        private static int BUFFER_SIZE = 1024 * 1024 * 100; //1KB x 1,024 = 1MB; 1MB x 100 = 100MB
        private const int PORT = 25565;

        static void Main(string[] args)
        {
            Console.Title = "Client";
            Console.SetWindowSize(80, 5);
            Directory.CreateDirectory(PATH);
            Directory.CreateDirectory(PATH0);
            while (!clientSocket.Connected)
                try
                {
                    Console.WriteLine("Trying to connect");
                    clientSocket.Connect("172.20.8.252", PORT);
                }
                catch (SocketException)
                {
                    Console.Clear();
                }
            Console.Clear();
            Console.WriteLine("Connected");

            while (true)
            {
                Thread t =  new Thread(() => ReceiveMessage());
                t.Start();
                ReadCommand();
            }
        }

        private static void ReadCommand()
        {
            Console.Write("Type: ");
            string message = Console.ReadLine();
            if (message == "push")
                Send();
            else
            {
                byte[] clientData = Encoding.ASCII.GetBytes(message);
                clientSocket.Send(clientData);
            }
            Console.Clear();
        }

        public static void Send()
        {
            foreach (string f in Directory.GetFiles(PATH))
            {
                byte[] fileNameByte = Encoding.ASCII.GetBytes(f);
                byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
                byte[] fileData = File.ReadAllBytes(f);
                byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];

                fileNameLen.CopyTo(clientData, 0);
                fileNameByte.CopyTo(clientData, 4);
                fileData.CopyTo(clientData, 4 + fileNameByte.Length);
                clientSocket.Send(clientData);
            }
        }

        private static void ReceiveMessage()
        {
            byte[] clientData = new byte[BUFFER_SIZE];
            int receivedBytesLen = clientSocket.Receive(clientData);
            int fileNameLen = BitConverter.ToInt32(clientData, 0);
            string fileName = new DirectoryInfo(Encoding.ASCII.GetString(clientData, 4, fileNameLen)).Name;
            BinaryWriter bWrite = new BinaryWriter(File.Open(PATH0 + @"\" + fileName, FileMode.Create));
            //BinaryWriter bWrite = new BinaryWriter(File.Open(@"C:\Users\panda\Desktop\Downloads\" + fileName, FileMode.Create));
            bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
            bWrite.Close();
        }
    }
}
