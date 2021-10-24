using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using _Defines;
using System.Data.SQLite;
using Dungeoneering_Server.Repository;
using System.Security.Cryptography;

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
        public static bool requesting = false;
        public static int parties = 0;
        public static DatabaseRepository repo;
        private static DatabaseProvider provider = new DatabaseProvider("Data Source=Database.db;Version=3;New=true");
        private static DatabaseMapper mapper = new DatabaseMapper();

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

            repo = new DatabaseRepository(provider, mapper);
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

            Player_Client player = new Player_Client(client, "", "", "", "", 1, 1,1);
            allStreams.Add(stream);

                stream = client.GetStream();
                byte[] msg = System.Text.Encoding.ASCII.GetBytes("Please write your name :)");

                //send back a response
                stream.Write(msg, 0, msg.Length);

            var name = "";
            bool accountAlreadyExist = false;
            

            while (stream != null)
            {
                if (name == "")
                {
                    name = recieveData(stream);
                    
                    for (int i = 0; i < repo.GetAllAccounts(client).Count; i++)
                    {
                        if(name == repo.GetAllAccounts(client)[i].character.name)
                        {
                            player = repo.GetAllAccounts(client)[i];
                            accountAlreadyExist = true;
                        }
                    }
                    if (accountAlreadyExist)
                    {
                        LogIn(player);
                    }
                    if (!accountAlreadyExist)
                    {
                        player.character.name = name;
                        CreateAccount(player);

                    }
                    generatePlayer(repo.FindAccount(name, client).client, player.client.Client.RemoteEndPoint.ToString(), player.character.name, player.character.password, player.character.salt, player.character.Level, player.character.str, player.character.dex);
                    foreach (var item in allPlayers)
                    {
                        if (item.client == client)
                        {
                            player = item;
                        }
                    }
                    allNames.Add(name);
                    _Helper.SendMessageToClient(client,"current commands : parties,create party,join party,stats");
                }
                    player.input = "";
                    string recievedData = recieveDataFromPlayer(stream,player);
                    Console.WriteLine($"{client.Client.RemoteEndPoint} >> {recievedData}");
                    SendData(recievedData, stream, name, client);

            }
        }

        public static void generatePlayer(TcpClient client,string ip,string name, string password, string salt, int level, int damage, int dex)
        {
            allPlayers.Add(new Player_Client(client, ip, name, password, salt, damage, dex, level));
        }
        public static string recieveDataFromPlayer(NetworkStream stream,Player_Client player)
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

            player.input = data;

            return data;
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
        }

        

        public static void dungeonStart(Lobby lobby)
        {
            Dungeon dungeon = new Dungeon(lobby);
        }

        public static bool PreCommands(string message, TcpClient client, NetworkStream stream, string name)
        {
            bool dungeonOrNot = false;

            foreach (var item in allPlayers)
            {
                if (item.client == client)
                {
                    if (item.Dungeoneering ==true)
                    {
                        dungeonOrNot = true;
                    }
                    else
                    {
                        dungeonOrNot = false;
                    }
                }
            }
            switch (message)
            {
                case "stats":
                    CharacterStats(client);
                    return false;
            }
            if (dungeonOrNot == false)
            {
                switch (message)
                {
                    case "dungeon":
                        int listnumber = 0;
                        for (int i = 0; i < ListOfLobbies.Count; i++)
                        {
                            for (int j = 0; j < ListOfLobbies[i].Players.Count; j++)
                            {
                                if (ListOfLobbies[i].Players[j].client == client)
                                {
                                    listnumber = i;
                                }
                            }

                        }
                        Thread z = new Thread(() => dungeonStart(ListOfLobbies[listnumber]));
                        z.IsBackground = true;
                        z.Start();
                        //Dungeon dungeon = new Dungeon(client, stream, name, ListOfLobbies[listnumber]);
                        return false;

                    case "parties":
                        if (ListOfLobbies.Count > 0)
                        {
                            foreach (var item in ListOfLobbies)
                            {
                                _Helper.SendMessageToClient(client, $"{item.name} {item.Players.Count}/3 \n");
                            }
                        }
                        else
                        {
                            _Helper.SendMessageToClient(client, "there is no parties currently available \n" +
                                "to make a new party write : create party");
                        }
                        return false;

                    case "create party":
                        ListOfLobbies.Add(new Lobby($"Party {parties}"));
                        //int number = 0;
                        //for (int i = 0; i < allPlayers.Count; i++)
                        //{
                        //    if(client == allPlayers[i].client)
                        //    {
                        //        number = i;
                        //    }
                        //}
                        //ListOfLobbies[parties].Players.Add(allPlayers[number]);
                        parties += 1;
                        _Helper.SendMessageToClient(client, "Party create \n" +
                            "to join a party write >join party<, to see a list of parties write >parties< ");
                        return false;

                    case "join party":
                        if (ListOfLobbies.Count > 0)
                        {
                            joinParty(client);
                        }
                        else
                        {
                            _Helper.SendMessageToClient(client, "No Parties available");
                        }
                        return false;

                    case "fight":
                        return false;

                    case "run":
                        return false;

                    case "attack":
                        return false;
                    case "leave party":
                        LeaveParty(client);
                        return false;
                    case "remove":
                        repo.RemovePlayer(repo.FindAccount(name, client).character.name, repo.FindAccount(name, client).client);
                        client.Client.Disconnect(false);
                        Environment.Exit(0);
                        return false;
                }
            }
            

            return true;
        }

        public static void CharacterStats(TcpClient client)
        {
            foreach (var item in allPlayers)
            {
                if (item.client == client)
                {
                    var pla = item.character;


                    _Helper.SendMessageToClient(client, $"Name : {pla.name} \n" +
                        $"Exp : {pla.exp} \n" +
                        $"Level : {pla.Level} \n" +
                        $"HP : {pla.hp} \n" +
                        $"str : {pla.str} \n" +
                        $"dex : {pla.dex} \n");



                }
            }
        }


        private static void CreateAccount(Player_Client player)
        {
            byte[] pass = Encoding.UTF8.GetBytes("What would you like as your password?");
            player.client.GetStream().Write(pass, 0, pass.Length);

            string password = recieveData(player.client.GetStream());

            byte[] hashedpassword = Hashing(password);

            byte[] salt = CreateSalt();

            string seed = Convert.ToBase64String(salt);

            string lockedpassword = Convert.ToBase64String(hashedpassword) + Convert.ToBase64String(salt);

            player.character.password = lockedpassword;
            player.character.salt = seed;

            repo.AddNewClient(player.character.name, player.character.password, player.character.salt, player.character.Level, player.character.damage, player.character.hp, player.character.dex);
        }

        private static void LogIn(Player_Client player)
        {
            byte[] pass = Encoding.UTF8.GetBytes("Type in the password");
            player.client.GetStream().Write(pass, 0, pass.Length);

            string password = recieveData(player.client.GetStream());

            byte[] hashingPassword = Hashing(password);
            string fullPassword = Convert.ToBase64String(hashingPassword) + player.character.salt;
            if (fullPassword == player.character.password)
            {
                player = repo.FindAccount(player.character.name, player.client);
            }
            else
            {
                while (true)
                {
                    byte[] tryAgain = Encoding.UTF8.GetBytes("That is not the right password, type 1 to try again and 2 to create a new account");
                    player.client.GetStream().Write(tryAgain, 0, tryAgain.Length);

                    string choice = recieveData(player.client.GetStream());
                    if (choice == "1")
                    {
                        LogIn(player);
                        break;
                    }
                    if (choice == "2")
                    {
                        byte[] newName = Encoding.UTF8.GetBytes("What would you like as your name?");
                        player.client.GetStream().Write(newName, 0, newName.Length);
                        string name = recieveData(player.client.GetStream());
                        player.character.name = name;
                        CreateAccount(player);
                        break;
                    }
                }

            }
        }

        public static void joinParty(TcpClient client)
        {
            _Helper.SendMessageToClient(client, "Here is a list of parties :\n");

            foreach (var item in ListOfLobbies)
            {
                _Helper.SendMessageToClient(client, $"{item.name} \n");
            }

            _Helper.SendMessageToClient(client, "Write the number of the party to join \n" +
                "For example to join party 0 write 0");

            var partyNumber = recieveData(client.GetStream());

            int result = Int32.Parse(partyNumber);

            foreach (var item in allPlayers)
            {
                if (item.client == client)
                {
                    ListOfLobbies[result].join(item);
                    //ListOfLobbies[result].Players.Add(item);
                    //ListOfLobbies.Find(x => x.name.Contains($"Party {partyNumber}"));
                }
            }
            _Helper.SendMessageToClient(client,$"You have joined Party {partyNumber}");
        }

        public static void LeaveParty(TcpClient client)
        {
            string lob = "";
            Player_Client player = new Player_Client(client, "1", "k", "", "", 1, 2,1);
            foreach(var item in allPlayers)
            {
                if (item.client == client)
                {
                    player = item;
                    lob = ListOfLobbies.Find(x => x.Players.Contains(item)).name;
                }
            }

            foreach (var item in ListOfLobbies)
            {
                if (item.name == lob)
                {
                    item.Players.Remove(player);
                }
            }

            _Helper.SendMessageToClient(client,"You have left the party");
        }

        private static byte[] Hashing(string password)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] decprytedPassword = Encoding.UTF8.GetBytes(password);

                return mySHA256.ComputeHash(decprytedPassword);
            }
        }

        private static byte[] CreateSalt()
        {
            using (var r = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[10];
                r.GetBytes(salt);
                return salt;
            }

        }
    }
}
