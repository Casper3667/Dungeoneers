using System;
using System.Net.Sockets;
using Dungeoneering_Server;
using System.Text;
using System.Collections.Generic;

namespace _Defines
{
    public static class _Helper
    {
        public static bool CheckHP(int hp)
        {
            if (hp <= 0)
                return false;
            else
                return true;
        }
        public static void SendMessageToAll(string message)
        {
            var mes =  message.ToLower();

            foreach (var item in Program.allUsers)
            {
                  var stream = item.GetStream();
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mes);

                    //send back a response
                    stream.Write(msg, 0, msg.Length);
            }
        }

        public static void SendMessageToAllInParty(string message, List<Player_Client> players)
        {
            var mes = message.ToLower();

            foreach (var item in players)
            {
                var stream = item.client.GetStream();
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mes);

                //send back a response
                stream.Write(msg, 0, msg.Length);
            }
        }

        public static void SendMessageToClient(TcpClient client, string message)
        {
            NetworkStream stream = client.GetStream();

            stream = client.GetStream();
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);

            //send back a response
            stream.Write(msg, 0, msg.Length);
        }

        /// <summary>
        /// Gives a nicely sorted message with every item in a list included.
        /// </summary>
        /// <typeparam name="T">List type</typeparam>
        /// <param name="input">The list to be given back</param>
        /// <param name="nothing_text">Result in case of empty list</param>
        /// <param name="and_text">The message between second to last and last item on the list</param>
        /// <param name="comma_text">The divider between items on the list</param>
        /// <returns>returns a sorted string with each item on the list</returns>
        public static string NiceList<T>(List<T> input, string nothing_text = "no result", string and_text = ",\n", string comma_text = ",\n")
        {
            string result = "";
            switch (input.Count)
            {
                case 0:
                    return nothing_text;
                case 1:
                    result = input[0].ToString();
                    break;
                case 2:
                    result = $"{input[0].ToString()}{and_text}{input[1].ToString()}";
                    break;
                default:
                    int count = 0;
                    foreach (T item in input)
                    {
                        count++;
                        if (count == input.Count)
                            result += $"{and_text}{item.ToString()}";
                        else if (count == 1)
                            result += $"{item.ToString()}";
                        else
                            result += $"{comma_text}{item.ToString()}";
                    }
                    break;
            }
            return result;
        }

    }
}
