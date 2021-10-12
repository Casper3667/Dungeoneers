﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using _Defines;

namespace Dungeoneering_Server
{
    class Program
    {
        public static List<TcpClient> allUsers = new List<TcpClient>();
        public static List<NetworkStream> allStreams = new List<NetworkStream>();
        public static List<Player_Client> allPlayers = new List<Player_Client>();
        public static List<Lobby> ListOfLobbies = new List<Lobby>();
        public static List<string> allNames = new List<string>();
        private static TcpListener server;
        private static Dungeon dungeon;
        public static bool requesting = false;
        public static int parties = 0;
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
            allStreams.Add(stream);

                stream = client.GetStream();
                byte[] msg = System.Text.Encoding.ASCII.GetBytes("Please write your name :)");

                //send back a response
                stream.Write(msg, 0, msg.Length);

            var name = "";

            while (stream != null)
            {
                if (name == "")
                {
                    name = recieveData(stream);
                    generatePlayer(client,client.Client.RemoteEndPoint.ToString(),name);
                    allNames.Add(name);
                }
                string recievedData = recieveData(stream);
                Console.WriteLine($"{client.Client.RemoteEndPoint} >> {recievedData}");
                SendData(recievedData, stream, name, client);
                
            }
        }

        public static void generatePlayer(TcpClient client,string ip,string name)
        {
            allPlayers.Add(new Player_Client(client,ip,name,1,1));
        }

        public static string recieveData(NetworkStream stream)
        {
            int i;
            Byte[] bytes = new Byte[256];
            String data = null;


            //byte[] tempbytes = new Byte[1];
            //int bytesRead = stream.Read(tempbytes);

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    data = data.ToLower();
                    break;
                }
            

           
            return data;
        }

        public static void SendData(string recievedData, NetworkStream stream,string name,TcpClient client)
        {
            var sendData = recievedData.ToLower();
            bool communication = PreCommands(sendData, client, stream, name);
            if (communication)
            {
                string chat = $"{name} >>> {recievedData}";

                chat.ToLower();

                foreach (var item in allUsers)
                {
                    if (client.Client.RemoteEndPoint != item.Client.RemoteEndPoint)
                    {

                        stream = item.GetStream();
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(chat);

                        //send back a response
                        stream.Write(msg, 0, msg.Length);
                    }
                }
            }
            if(!communication)
            {
                byte[] msg = Encoding.ASCII.GetBytes(recievedData);
                stream.Write(msg, 0, msg.Length);
            }
            

            //switch (sendData)
            //{
            //    case "dungeon":
            //        dungeon = new Dungeon(client, stream, name);
                    
            //        break;
            //    case "preparing":

            //        break;
            //    case "dun":

            //        break;
            //    case "fuck":

            //        break;
            //}
        }

        public static bool PreCommands(string message, TcpClient client, NetworkStream stream, string name)
        {
            switch (message)
            {
                case "dungeon":
                    dungeon = new Dungeon(client, stream, name);
                    return false;

                case "parties":
                    return false;

                case "Y":
                    return false;
                    
                case "fuck":
                    return false;

                case "fight":
                    return false;

                case "run":
                    return false;
            }

            return true;
        }
    }
}
