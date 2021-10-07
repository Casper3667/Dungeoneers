using _Defines;

namespace Dungeon
{
    internal class Character
    {
        protected string name;
        protected int damage;
        protected int hp;
        protected int armor;

        private bool CheckDeath()
        {
            return _Helper.CheckHP(hp);
        }

        public bool TakeDamage(int damageTaken)
        {
            damageTaken -= armor;
            if (damageTaken <= 0)
                return true;
            return CheckDeath();
        }
    }
}
