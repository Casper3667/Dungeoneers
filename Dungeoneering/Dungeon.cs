using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Net;
using Dungeoneering_Server;
using _Defines;
using System.Threading;
using Items;

namespace Dungeoneering_Server
{
    class Dungeon
    {
        private Random r;
        private TcpClient client;
        private NetworkStream stream;
        private Lobby players;
        private string name;
        private string message;
        private int fight = 0;
        private int run = 0;
        public static string result;

        public static string[] WeaponAffixes =  {"fire","water"};
        

        public Dungeon(Lobby players)
        {
            r = new Random();
            this.players = players;

            ChooseLevel();

        }

        private void ChooseLevel()
        {
            var leader = players.Players[0];
            string leaderString = $"{leader.character.name} Is the leader \n" +
                $"leader is choosing dificulty \n";
            _Helper.SendMessageToAllInParty(leaderString, players);

            string dungeons = $"Avilable Dungeons : \n" +
                $"Dungeon 1 > lvl 1 - 3 \n" +
                $"Dungeon 2 > lvl 2 - 6 \n" +
                $"Dungeon 3 > lvl 3 - 9 \n" +
                $"there is no max for dungeon level, these are examples, 50 is a viable input \n" + 
                $"to pick a dungeon, write the number of the associated dungeon, dungeon 1 would for example be 1 \n";

            _Helper.SendMessageToClient(leader.client, dungeons);

            int result = Int32.Parse(expectingMessage(leader));

            Random rnd = new Random();
            int level = rnd.Next(result,result*3);

            Quest(level);
        }

        private void Quest(int level)
        {
            int monsterLevel = level;
            int tier = r.Next(1, 4);
            if(tier == 1)
            {
                message = $"You are fighting a lvl {monsterLevel} goblin \n";
            }
            if (tier == 2)
            {
                message = $"You are fighting a lvl {monsterLevel} orc \n";
            }
            if (tier == 3)
            {
                message = $"You are fighting a lvl {monsterLevel} dragon \n";
            }
            _Helper.SendMessageToAllInParty(message, players);


            message = "Party deciding whetever to fight or run.....";
            _Helper.SendMessageToAllInParty(message, players);


            string fightOrRun = MessageReceiver();
            message = $"The majority have chosen to {fightOrRun}";
            _Helper.SendMessageToAllInParty(message, players);
            if(fightOrRun == "fight")
            {
                Combat(monsterLevel);
            }
            else
            {
                string left = $"The majority have chosen to run, return to lobby screen \n";
                _Helper.SendMessageToAllInParty(left, players);
            }


        }

        public static string expectingMessage(Player_Client player)
        {
            string mes;
            
            while (player.input == "")
            {

            }

            mes = player.input;

            return mes;
        }


        private string MessageReceiver()
        {
            for (int i = 0; i < players.Players.Count; i++)
            {
                string fightOrNot = "fight or run?";
                _Helper.SendMessageToClient(players.Players[i].client,fightOrNot);
                string warroirsMessage = expectingMessage(players.Players[i]);
                //string warroirsMessage = Program.recieveData(players.Players[i].client.GetStream());
                if (warroirsMessage == "fight")
                {
                    fight++;
                }

                if (warroirsMessage == "run")
                {
                    run++;
                }


            }

            if (fight > run)
            {
                return "fight";
            }
            else
            {
                return "run";
            }
        }

       

       

        private void Combat(int monsterlevel)
        {
            bool partyDeath = false;
            int teamHealth = 0;
            for (int i = 0; i < players.Players.Count; i++)
            {
                players.Players[i].Dungeoneering = true;
                teamHealth += players.Players[i].character.hp;
            }
            int monsterhealth = 30*monsterlevel;
            int monsterDmg = 5 * monsterlevel;


            string monsterStats = $"Monster stats : \n " +
                $"level : {monsterlevel} \n" +
                $"health : {monsterhealth} \n" +
                $"damage : {5} - {monsterDmg}\n";

            _Helper.SendMessageToAllInParty(monsterStats, players);
            
            while(monsterhealth > 0 /*&& teamHealth > 0*/)
            {
                int count = 0;
                
                foreach (var item in players.Players)
                {
                    
                    if (item.character.hp <= 0)
                    {
                        count += 1;
                    }
                }

                if (count >= players.Players.Count)
                {
                    partyDeath = true;
                    break;
                }
                for (int i = 0; i < players.Players.Count; i++)
                {
                    for (int j = 0; j < players.Players.Count; j++)
                    {
                        if (j != i)
                        {
                            string otherMessage = $"It is currently {players.Players[i].character.name}'s turn \n";
                            _Helper.SendMessageToAllInParty(otherMessage,players);
                        }
                    }
                    string mes = $"{players.Players[i].character.name} choose your action,\n";
                    _Helper.SendMessageToClient(players.Players[i].client, mes);
                    mes = $"available actions : attack \n";
                    _Helper.SendMessageToClient(players.Players[i].client, mes);

                    //Thread z = new Thread(() => DrecieveData(ListOfLobbies[listnumber]));
                    //z.IsBackground = true;
                    //z.Start();

                    while (true)
                    {
                        string choice = expectingMessage(players.Players[i]);
                        if (choice == "attack")
                        {
                            var dmg = players.Players[i].character.Attack();
                            string action = $"{players.Players[i].character.name} has choosen to Melee attack{players.Players[i].character.AttackRange()} for {dmg} damage \n";
                            _Helper.SendMessageToAllInParty(action, players);
                            monsterhealth -= dmg;
                            string monsterFeedback = $"the monster has {monsterhealth} hp left \n";
                            _Helper.SendMessageToAllInParty(monsterFeedback, players);
                            break;

                        }
                        
                    }

                }
                r = new Random();
                int playerToAttack = r.Next(1, players.Players.Count + 1);
                if (monsterhealth >0)
                {
                    var monsterRoll = r.Next(5, monsterDmg);
                    string monstersTurn = $"The monster is attacking {players.Players[playerToAttack - 1].character.name}, they took {monsterRoll} damage \n";
                    players.Players[playerToAttack - 1].character.hp -= monsterRoll;
                    _Helper.SendMessageToAllInParty(monstersTurn, players);
                    string healtLeft = $"{players.Players[playerToAttack - 1].character.name} has {players.Players[playerToAttack - 1].character.hp}hp left \n";
                    _Helper.SendMessageToAllInParty(healtLeft, players);
                }
                if(players.Players[playerToAttack - 1].character.hp <= 0)
                {
                    string death = $"{players.Players[playerToAttack - 1].character.name} has died";
                    _Helper.SendMessageToAllInParty(death, players);
                }
            }

            foreach (var item in players.Players)
            {
                item.Dungeoneering = false;
                if (partyDeath == false)
                {
                    item.character.GaintExperience(5 * monsterlevel);
                }
            }

            if (partyDeath == true)
            {
                foreach (var item in players.Players)
                {
                    item.character.hp = item.character.maxHP;
                }
                string allDeadLul = "all players have died, return to lobby";
                _Helper.SendMessageToAllInParty(allDeadLul, players);
            }
            if (partyDeath == false)
            {
                string ret = $"Monster has died, returning to lobby screen, {5*monsterlevel}xp rewarded \n";
                _Helper.SendMessageToAllInParty(ret, players);

                var wpn = GenerateWeapon(monsterlevel);
                string weaponMes = $"The Monster Dropped an item! : " +
                    $"{wpn.Name} has dropped \n" +
                    $"Damage :  {wpn.damage} \n" +
                    $"\n" +
                    $"game will go through each party member and ask if they want it, if they yes it will replace their current weapon \n" +
                    $"if they so no it will ask the next player \n" +
                    $"if all decline weapon will go lost \n";
                _Helper.SendMessageToAllInParty(weaponMes, players);

                bool ctinue = true;
                for (int i = 0; i < players.Players.Count; i++)
                {
                    if (ctinue == true)
                    {
                        string pturn = $"it's currently {players.Players[i].character.name} turn to choose \n";
                        _Helper.SendMessageToAllInParty(pturn, players);

                        while (true)
                        {
                            string doYouWantIt = $"Do you want the weapon? Y/N \n";
                            _Helper.SendMessageToClient(players.Players[i].client, doYouWantIt);


                            string answer = expectingMessage(players.Players[i]);
                            answer = answer.ToLower();
                            if (answer == "y")
                            {
                                players.Players[i].character.changeWeapon(wpn);
                                ctinue = false;

                                string pTookWeapon = $"{players.Players[i].character.name} picked up the weapon \n";
                                Program.CollectWeapon(players.Players[i], wpn.Name, wpn.damage, wpn.prefix);
                                _Helper.SendMessageToAllInParty(pTookWeapon, players);
                                break;
                            }
                            else if (answer == "n")
                            {
                                break;
                            }
                        }
                    }
                    
                }
                for (int i = 0; i < players.Players.Count; i++)
                {
                    players.Players[i].character.hp = players.Players[i].character.maxHP;
                }
                    string again = $"Do the party wish to continue exploring? Leader please choose \n" +
                    $"Leader is : {players.Players[0].character.name} \n";
                _Helper.SendMessageToAllInParty(again, players);


                while (true)
                {

                    string leaderOnly = "Do you wish to continue? \n" +
                        "yes or no?";
                    _Helper.SendMessageToClient(players.Players[0].client, leaderOnly);

                    string Expa = expectingMessage(players.Players[0]);
                    Expa = Expa.ToLower();
                    if (Expa == "yes")
                    {
                        ChooseLevel();
                    }
                    else if (Expa == "no")
                    {
                        string ratata = $"returning to lobby screen";
                        _Helper.SendMessageToAllInParty(ratata, players);
                        break;
                    }
                }

                
            }


        }


        public Weapon GenerateWeapon(int level)
        {
            Random rnd = new Random();
            int dmg = level;
            var name = $"{WeaponAffixes[0]} Sword";
            Weapon tempWEapon = new Weapon(name, dmg, "", "");

            return (tempWEapon);
        }

    }
}
