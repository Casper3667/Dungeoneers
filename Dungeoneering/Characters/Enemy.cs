using _Defines;

namespace Dungeon
{
    internal class Enemy : Character
    {
        public Enemy(string _name, _Defines._Defines.EnemyDamage _damage, _Defines._Defines.EnemyHP _hp, _Defines._Defines.EnemyArmor _armor)
        {
            name = _name;
            damage = (int)_damage;
            hp = (int)_hp;
            armor = (int)_armor;
        }

        public void Attack(Character target)
        {
            target.TakeDamage(damage);
        }
    }
}
