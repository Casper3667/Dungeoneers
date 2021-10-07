using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;

namespace Dungeoneering_Client
{
    class Program
    {
        static TcpClient client = null;
        static NetworkStream stream = null;
        static byte[] message = null;
        static bool waiting = false;
        static string IP;
        static void Main(string[] args)
        {
            Connect();            
        }

        private static void Connect()
        {
            try
            {
                Console.WriteLine("Type the IP of the server you wanna join");
                string ip = Console.ReadLine();
                IP = ip;
                
                
                while (true)
                {
                    if(!waiting)
                    {
                        Sending();
                    }
                    if (waiting)
                    {
                        Receiving();
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("You did a wrong input");
            }
            
        }

        private static void Sending()
        {
            client = new TcpClient(IP, 42069);
            stream = client.GetStream();
            bool dungeon = false;
            Console.WriteLine("Choice your actions");
            Console.WriteLine("Possible actions are : Preparing (waiting for comrades), Dungeon (go on a dungeon), Stats (inspect your stats)");
            string action = Console.ReadLine();
            action.ToUpper();
            if (action == "PREPARING")
            {
                message = Encoding.ASCII.GetBytes(action);
                stream.Write(message, 0, message.Length);

            }
            if (action == "DUNGEON")
            {
                message = Encoding.ASCII.GetBytes(action);
                stream.Write(message, 0, message.Length);
                dungeon = true;
            }
            if (action == "STATS")
            {
                message = Encoding.ASCII.GetBytes(action);
                stream.Write(message, 0, message.Length);
            }
            if(dungeon)
            {

            }
            //waiting = true;
        }

        private static void Receiving()
        {
            client = new TcpClient(IP, 42069);
            stream = client.GetStream();
            message = new byte[256];
            Int32 receiver = stream.Read(message, 0, message.Length);
            string serverResponse = Encoding.ASCII.GetString(message, 0, receiver);
            Console.WriteLine("Message from the client " + serverResponse);

            if(serverResponse == null)
            {
                waiting = false;
            }
        }
    }
}
