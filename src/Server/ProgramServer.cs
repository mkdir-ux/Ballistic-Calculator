using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class NetworkListener
{
    static void Main()
    {
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Loopback, 8888);
        TcpListener server = new TcpListener(localEndPoint);

        server.Start();
        Console.WriteLine("Server started. Waiting for your Calculator data...");

        using (TcpClient client = server.AcceptTcpClient())
        using (NetworkStream stream = client.GetStream())
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine($"[SUCCESS] Data received: {receivedData}");
        }
        server.Stop();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}