using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Dungeoneering_Server
{
    class Program
    {
        public static List<TcpClient> allUsers = new List<TcpClient>();
        private static TcpListener server;

        static void Main(string[] args)
        {
            try
            {
                server = new TcpListener(IPAddress.Any, 42069);
                server.Start();

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            ServerStart(server);

            AcceptNewClients(server);

        }

        static void ServerStart(TcpListener server)
        {
            

            Console.WriteLine("Server Started----------------------");

            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            Console.WriteLine($"Server IP: {localIP}");
        }

        private static void AcceptNewClients(TcpListener server)
        {
            Console.WriteLine("Wait for new client........");
            while (true)
            {
                TcpClient client  = server.AcceptTcpClient();

                Console.WriteLine($"--->> {client.Client.RemoteEndPoint} <<---  Connected");

                Thread t = new Thread(HandleClient);

                allUsers.Add(client);

                t.IsBackground = true;
                t.Start(client);
            }
        }

        private static void HandleClient(object data)
        {
            TcpClient client = (TcpClient)data;
            NetworkStream stream = client.GetStream();


            while (true)
            {
                string recievedData = recieveData(stream);
                Console.WriteLine($"{client.Client.RemoteEndPoint} >> {recievedData}");
            }
        }

        public static string recieveData(NetworkStream stream)
        {
            int i;
            Byte[] bytes = new Byte[256];
            String data = null;

            while ((i = stream.Read(bytes,0,bytes.Length))!=0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes,0,i);
                Console.WriteLine($"recieved : {data}");

                data = data.ToLower();
            }

            return data;
        }
    }
}
