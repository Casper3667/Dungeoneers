using Dungeon;
using System.Collections.Generic;

namespace _Defines
{
    public static class _Defines
    {
        private static string ORC = "string";
        public enum EnemyDamage
        {
            ORC = 3
        }
        public enum EnemyHP
        {

            ORC = 10
        }
        public enum EnemyArmor
        {
            ORC = 0
        }

        private static List<Enemy> Enemies = new List<Enemy>();

        public static void Initialize()
        {
            Enemy Orc = new Enemy(ORC, EnemyDamage.ORC, EnemyHP.ORC, EnemyArmor.ORC);
            Enemies.Add(Orc);
        }
    }
}
