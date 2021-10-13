using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Net;
using Dungeoneering_Server;
using _Defines;

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
        

        public Dungeon(TcpClient client, NetworkStream stream, string name, Lobby players)
        {
            r = new Random();
            this.client = client;
            this.stream = stream;
            this.name = name;
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

        private string MessageReceiver()
        {
            for (int i = 0; i < players.Players.Count; i++)
            {
                string warroirsMessage = Program.recieveData(players.Players[i].client.GetStream());
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
                teamHealth += players.Players[i].character.hp;
            }
            int monsterhealth = 5;
            
            while(monsterhealth > 0 /*&& teamHealth > 0*/)
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
                    string choice = Program.recieveData(players.Players[i].client.GetStream());
                    string action = $"{players.Players[i].character.name} has choosen to {choice} with {players.Players[i].character.damage} damage";
                    _Helper.SendMessageToClient(players.Players[i].client, action);

                }
                r = new Random();
                int playerToAttack = r.Next(1, players.Players.Count + 1);
                string monstersTurn = $"The monster is attacking {players.Players[playerToAttack - 1].character.name}";
                _Helper.SendMessageToAllInParty(monstersTurn, players);
                if(players.Players[playerToAttack - 1].character.hp <= 0)
                {
                    string death = $"{players.Players[playerToAttack - 1].character.name} has died";
                    _Helper.SendMessageToAllInParty(death, players);
                }
            }

        }

    }
}
