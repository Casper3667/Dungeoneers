using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

namespace Dungeoneering_Client
{
    class Program
    {
        static TcpClient client;
        static NetworkStream stream = null;
        static byte[] message = null;
        static bool waiting = true;
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
                client = new TcpClient(IP, 42069);

                Thread t = new Thread(Receiving);
                t.IsBackground = true;
                t.Start(client);

                while (true)
                {
                    if(waiting)
                    {
                        Sending();
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
            stream = client.GetStream();
            string action = Console.ReadLine();
            action = action.ToUpper();
            message = Encoding.ASCII.GetBytes(action);
            stream.Write(message, 0, message.Length);
        }

        private static void Receiving(object data)
        {
            while (true)
            {
                stream = client.GetStream();
                byte[] m = new byte[256];
                Int32 receiver = stream.Read(m, 0, m.Length);
                string serverResponse = Encoding.ASCII.GetString(m, 0, receiver);
                Console.WriteLine(serverResponse);
            }
            
        }
    }
}
