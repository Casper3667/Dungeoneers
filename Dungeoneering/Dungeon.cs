using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Net;
using Dungeoneering_Server;
using _Defines;
using System.Threading;

namespace Dungeoneering_Server
{
    class Dungeon
    {
        private Random r;
        private TcpClient client;
        private NetworkStream stream;
        private Lobby players;
        private string name;
        private string message;
        private int fight = 0;
        private int run = 0;
        public static string result;
        

        public Dungeon(Lobby players)
        {
            r = new Random();
            this.players = players;

            Quest();

        }

        private void Quest()
        {
            int tier = r.Next(1, 4);
            if(tier == 1)
            {
                message = "You are fighting a goblin";
            }
            if (tier == 2)
            {
                message = "You are fighting a orc";
            }
            if (tier == 3)
            {
                message = "You are fighting a dragon";
            }

            _Helper.SendMessageToAllInParty(message, players);


            message = "You wanna fight or run?";
            _Helper.SendMessageToAllInParty(message, players);


            string fightOrRun = MessageReceiver();
            message = $"The majority have chosen to {fightOrRun}";
            _Helper.SendMessageToAllInParty(message, players);
            if(fightOrRun == "fight")
            {
                Combat();
            }

        }

        public static string expectingMessage(Player_Client player)
        {
            string mes;
            
            while (player.input == "")
            {

            }

            mes = player.input;

            return mes;
        }

        private string MessageReceiver()
        {
            for (int i = 0; i < players.Players.Count; i++)
            {
                string warroirsMessage = expectingMessage(players.Players[i]);
                //string warroirsMessage = Program.recieveData(players.Players[i].client.GetStream());
                if (warroirsMessage == "fight")
                {
                    fight++;
                }

                if (warroirsMessage == "run")
                {
                    run++;
                }


            }

            if (fight > run)
            {
                return "fight";
            }
            else
            {
                return "run";
            }
        }

       

       

        private void Combat()
        {
            int teamhealth = 25;
            int monsterhealth = 5;

            while(monsterhealth > 0 && teamhealth > 0)
            {
                for (int i = 0; i < players.Players.Count; i++)
                {
                    for (int j = 0; j < players.Players.Count; j++)
                    {
                        if (j != i)
                        {
                            string otherMessage = $"It is currently {players.Players[i].character.name}'s turn";
                            _Helper.SendMessageToClient(players.Players[j].client, otherMessage);
                        }
                    }
                    string mes = $"{players.Players[i].character.name} choice your action";
                    _Helper.SendMessageToClient(players.Players[i].client, mes);

                    //Thread z = new Thread(() => DrecieveData(ListOfLobbies[listnumber]));
                    //z.IsBackground = true;
                    //z.Start();

                    string choice = expectingMessage(players.Players[i]);
                    //string choice = Program.recieveData(players.Players[i].client.GetStream());
                    string action = $"{players.Players[i].character.name} is {choice} with damage amount";
                    _Helper.SendMessageToClient(players.Players[i].client, action);

                }
                r = new Random();
                int playerToAttack = r.Next(1, players.Players.Count + 1);
                string monstersTurn = $"The monster is attacking {players.Players[playerToAttack - 1].character.name}";
                _Helper.SendMessageToAllInParty(monstersTurn, players);
                Console.WriteLine("");
            }

        }

    }
}
