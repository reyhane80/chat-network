using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

public class TcpClientSample
{
    
    private const string serverIp = "127.0.0.1";
    private const int port = 5000;

    public static void Main()
    {
        TcpClient server;
        try
        {
            server = new TcpClient(serverIp, port);
        }
        catch (SocketException)
        {
            Console.WriteLine("Unable to connect to server");
            return;
        }
        Console.WriteLine("Connected to the Server...");
        NetworkStream ns = server.GetStream();

        SendFile("test.txt", ns);

        ns.Close();
        server.Close();
    }

    public static void SendFile(string fileName, NetworkStream ns)
    {

        byte[] fileData = File.ReadAllBytes(fileName);


        byte[] fileDataLength = BitConverter.GetBytes(fileData.Length);
        ns.Write(fileDataLength, 0, fileDataLength.Length);


        ns.Write(fileData, 0, fileData.Length);
        ns.Flush();
    }
}
