using System;
using System.Collections.Generic;
using System.Text;
using Items;
using _Defines;
using Dungeoneering_Server;
using System.Net.Sockets;

namespace Dungeon
{
    public class Player : Character
    {
        private TcpClient client;
        public int str;
        public int dex;
        public string password;
        public string salt;
        public List<Item> inventory;
        public Weapon sword;
        public Player(TcpClient client, string _name, string _password, string _salt, int _str, int _dex,int lvl)
        {
            this.client = client;
            Level = lvl;
            name = _name;
            password = _password;
            salt = _salt;
            str = _str;
            dex = _dex;
            //sword = Program.CheckForWeapon(_name);
            inventory = new List<Item>();
            inventory.Add(sword);

            this.damage = (str * 1);
            this.hp = (50 * Level);
            maxHP = hp;
            
        }
        public override void Attack(Character target)
        {
            int damage = str + sword.attack(); // placeholder until weapons are implemented
            target.TakeDamage(damage);
        }

        public void changeWeapon(Weapon weap)
        {
            sword = weap;
        }

        public int Attack()
        {
            Random rnd = new Random();

            var dmg = rnd.Next(str, (str*3));
            dmg += sword.attack();
            return dmg;
        }

        public string AttackRange()
        {
            string range = $"({str} - {str*3}) damage";

            return range;
        }

        public void TakeItem(Item target)
        {
            if (inventory.Contains(target))
                return;
            inventory.Add(target);
        }

        public string GetInventory()
        {
            return _Helper.NiceList<Item>(inventory);
        }

        public void GaintExperience(int xp)
        {
            exp += xp;

            if (exp > (50*Level))
            {
                Level += 1;

                str += 5;
                dex += 5;
                hp += 20;
                maxHP += 20;

               
                Program.LevelUp(name, client,this.Level,str,dex,maxHP);
                exp = 0;
            }   
        }
    }
}
