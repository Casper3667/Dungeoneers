using System;

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
        public static void SendMessage(string message)
        {
            Console.WriteLine(message);
        }

    }
}
