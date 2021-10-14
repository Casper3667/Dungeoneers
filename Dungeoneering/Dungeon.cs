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


            message = "Party deciding whetever to fight or run.....";
            _Helper.SendMessageToAllInParty(message, players);


            string fightOrRun = MessageReceiver();
            message = $"The majority have chosen to {fightOrRun}";
            _Helper.SendMessageToAllInParty(message, players);
            if(fightOrRun == "fight")
            {
                Combat();
            }
            else
            {
                string left = $"The majority have chosen to run, return to lobby screen \n";
                _Helper.SendMessageToAllInParty(left, players);
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
                string fightOrNot = "fight or run?";
                _Helper.SendMessageToClient(players.Players[i].client,fightOrNot);
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
            int teamHealth = 0;
            for (int i = 0; i < players.Players.Count; i++)
            {
                players.Players[i].Dungeoneering = true;
                teamHealth += players.Players[i].character.hp;
            }
            int monsterhealth = 30;
            int monsterDmg = 5;
            
            while(monsterhealth > 0 /*&& teamHealth > 0*/)
            {
                for (int i = 0; i < players.Players.Count; i++)
                {
                    for (int j = 0; j < players.Players.Count; j++)
                    {
                        if (j != i)
                        {
                            string otherMessage = $"It is currently {players.Players[i].character.name}'s turn \n";
                            _Helper.SendMessageToAllInParty(otherMessage,players);
                        }
                    }
                    string mes = $"{players.Players[i].character.name} choice your action,\n";
                    _Helper.SendMessageToClient(players.Players[i].client, mes);
                    mes = $"available actions : attack \n";
                    _Helper.SendMessageToClient(players.Players[i].client, mes);

                    //Thread z = new Thread(() => DrecieveData(ListOfLobbies[listnumber]));
                    //z.IsBackground = true;
                    //z.Start();

                    while (true)
                    {
                        string choice = expectingMessage(players.Players[i]);
                        if (choice == "attack")
                        {
                            string action = $"{players.Players[i].character.name} has choosen to Melee attack with {players.Players[i].character.damage} damage \n";
                            _Helper.SendMessageToAllInParty(action, players);
                            monsterhealth -= players.Players[i].character.damage;
                            string monsterFeedback = $"the monster has {monsterhealth} hp left \n";
                            _Helper.SendMessageToAllInParty(monsterFeedback, players);
                            break;

                        }
                        
                    }

                }
                r = new Random();
                int playerToAttack = r.Next(1, players.Players.Count + 1);
                if (monsterhealth >0)
                {
                    string monstersTurn = $"The monster is attacking {players.Players[playerToAttack - 1].character.name}, they took {monsterDmg} damage \n";
                    players.Players[playerToAttack - 1].character.hp -= monsterDmg;
                    _Helper.SendMessageToAllInParty(monstersTurn, players);
                    string healtLeft = $"{players.Players[playerToAttack - 1].character.name} has {players.Players[playerToAttack - 1].character.hp}hp left \n";
                    _Helper.SendMessageToAllInParty(healtLeft, players);
                }
                if(players.Players[playerToAttack - 1].character.hp <= 0)
                {
                    string death = $"{players.Players[playerToAttack - 1].character.name} has died";
                    _Helper.SendMessageToAllInParty(death, players);
                }
            }

            foreach (var item in players.Players)
            {
                item.Dungeoneering = false;
            }

            string ret = $"Monster has died, returning to lobby screen, xp rewarded \n";
            _Helper.SendMessageToAllInParty(ret, players);


        }

    }
}
