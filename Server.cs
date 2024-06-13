using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;

namespace ConsoleApp5
{
    class Program
    {
        static TcpListener tcpServer;
        static Thread th;

        public static void Main()
        {
            th = new Thread(new ThreadStart(StartListen));
            th.Start();
        }

        public static void StartListen()
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            tcpServer = new TcpListener(localAddr, 5000);
            tcpServer.Start();

            while (true)
            {
                TcpClient tcpClient = tcpServer.AcceptTcpClient();
                NetworkStream ns = tcpClient.GetStream();

                ReceiveFile(ns);

                ns.Close();
                tcpClient.Close();
            }
        }

        public static void ReceiveFile(NetworkStream ns)
        {

            byte[] fileDataLengthBytes = new byte[4];
            ns.Read(fileDataLengthBytes, 0, 4);
            int fileDataLength = BitConverter.ToInt32(fileDataLengthBytes, 0);

            byte[] fileData = new byte[fileDataLength];
            int bytesRead = 0;
            int totalBytesRead = 0;

            while (totalBytesRead < fileDataLength)
            {
                bytesRead = ns.Read(fileData, totalBytesRead, fileDataLength - totalBytesRead);
                totalBytesRead += bytesRead;
            }

            File.WriteAllBytes("received_file", fileData);
        }
    }
}
