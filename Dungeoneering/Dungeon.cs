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
        private string name;
        private string message;
        private int fight = 0;
        private int run = 0;

        public Dungeon(TcpClient client, NetworkStream stream, string name)
        {
            r = new Random();
            this.client = client;
            this.stream = stream;
            this.name = name;
            
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

            _Helper.SendMessageToAll(message);
         

            message = "You wanna fight or run?";
            _Helper.SendMessageToAll(message);


            string fightOrRun = MessageReceiver();
            message = $"The majority have chosen to {fightOrRun}";
            _Helper.SendMessageToAll(message);
            if(fightOrRun == "fight")
            {
                Combat();
            }

        }

        private string MessageReceiver()
        {
            for (int i = 0; i < Program.allUsers.Count; i++)
            {
                string warroirsMessage = Program.recieveData(Program.allUsers[i].GetStream());
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
                for (int i = 0; i < Program.allUsers.Count; i++)
                {
                    for (int j = 0; j < Program.allUsers.Count; j++)
                    {
                        if (j != 0)
                        {
                            string otherMessage = $"It is currently {Program.allNames[i]} turn";
                            _Helper.SendMessageToClient(Program.allUsers[j],otherMessage);
                        }
                    }
                    string mes = $"{Program.allNames[i]} choice your action";
                    _Helper.SendMessageToClient(Program.allUsers[i],mes);

                }
            }

        }

        private void MessageSender(string message, TcpClient client, NetworkStream stream, string name)
        {
            Program.SendData(message, stream, name, client);
        }
        
    }
}
