using _Defines;

namespace Dungeon
{
    public abstract class Character
    {
        protected int Level;
        protected int exp;
        public string name;
        protected int damage;
        protected int hp;
        protected int armor;
        protected string deathMessage;

        private bool CheckDeath()
        {
            return _Helper.CheckHP(hp);
        }

        public bool TakeDamage(int damageTaken)
        {
            damageTaken -= armor;
            if (damageTaken >= 0)
                hp -= damageTaken;
            bool living = CheckDeath();
            if (!living)
                _Helper.SendMessageToAll(deathMessage);
            return living;
        }

        public abstract void Attack(Character target);
    }
}
