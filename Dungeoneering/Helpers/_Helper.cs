using System;
using Dungeoneering_Server;

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

    }
}
