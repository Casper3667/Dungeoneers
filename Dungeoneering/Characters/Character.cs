using _Defines;

namespace Dungeon
{
    public abstract class Character
    {
        public int Level;
        public int exp;
        public string name;
        public int damage;
        public int hp;
        public int maxHP;
        public int armor;
        public string deathMessage;

        public bool CheckDeath()
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
