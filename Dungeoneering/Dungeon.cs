using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Dungeoneering_Server;

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

            Program.requesting = true;
            MessageSender(message);            

            message = "You wanna fight or run?";
            MessageSender(message);
            

            message = $"The majority have chosen to {MessageReceiver()}";
            MessageSender(message);
            Program.requesting = false;
        }

        private void MessageSender(string message)
        {
            for (int i = 0; i < Program.allUsers.Count; i++)
            {
                Program.SendData(message, Program.allStreams[i], name, Program.allUsers[i]);
            }
            
        }

        private string MessageReceiver()
        {

            //foreach (TcpClient clients in Program.allUsers)
            //{
            //    string decision = Program.recieveData(stream);
            //}
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
        
    }
}
