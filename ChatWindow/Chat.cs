using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatWindow
{
    internal class Chat
    {
        private static readonly Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static string PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\AppServer\Upload";
        private const int PORT = 25565;

        static void Main(string[] args)
        {
            Console.Title = "Chat Window";
            clientSocket.Connect("172.20.8.252", PORT);
            Console.Clear();
            while (true)
                ReceiveMessage();
        }

        private static void ReceiveMessage()
        {
            Console.WriteLine("Receiving file");
            byte[] clientData = new byte[1024 * 10000];
            int receivedBytesLen = clientSocket.Receive(clientData);
            int fileNameLen = BitConverter.ToInt32(clientData, 0);
            string fileName = new DirectoryInfo(Encoding.ASCII.GetString(clientData, 4, fileNameLen)).Name;
            //BinaryWriter bWrite = new BinaryWriter(File.Open(PATH + @"\" + fileName, FileMode.Create));
            BinaryWriter bWrite = new BinaryWriter(File.Open(@"C:\Users\panda\Desktop\Downloads\" + fileName, FileMode.Create));
            bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
            bWrite.Close();
            Console.WriteLine("Received file");
        }
    }
}
