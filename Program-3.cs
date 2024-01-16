using System;

namespace project
{
    public class Rand
    {
        public int Run(int min, int max)
        {
            int range = (max - min) + 1;
            Random rng = new Random();
            return min + rng.Next() % range;
        }
    }

    public class Hero
    {
        public string Name;
        private int Strength;
        private int Dexterity;
        private int Intelligence;
        public double HP;
        public double MP;

        private void Init(int strength = 10, int dexterity = 10, int intelligence = 10)
        {
            this.Strength = strength;
            this.Dexterity = dexterity;
            this.Intelligence = intelligence;
            HP = 50 + strength;
            MP = 10 + (3 * intelligence);
        }

        public int GetStrength() { return this.Strength; }
        public int GetDexterity() { return this.Dexterity; }
        public int GetIntelligence() { return this.Intelligence; }

        public void UpStrength() { this.Strength += 5; this.HP += 5; }
        public void UpDexterity() { this.Dexterity += 5; }
        public void UpIntelligence() { this.Intelligence += 5; this.MP += (3 * this.Intelligence); }

        public Hero(string name, string myclass)
        {
            Name = name;
            switch (myclass)
            {
                case "warior": Init(15, 10, 5); break;
                case "assassin": Init(5, 15, 10); break;
                case "sorcerer": Init(5, 5, 20); break;
                default: Init(); break;
            }
        }

        public void Attack(Hero enemy)
        {
            Rand rand = new Rand();
            double damage = Strength * rand.Run(5, 10) / 10;

            if (rand.Run(0, 100) > enemy.GetDexterity())
            {
                Console.WriteLine("Bang!");
                enemy.HP -= damage;
            }
            else Console.WriteLine("Dodge!");
        }

        public void LevelUp()
        {
            Console.Write("  1:Strength, 2:Dexterity, 3:Intelligence ... ");
            int opt = int.Parse(Console.ReadLine());

            switch (opt)
            {
                case 1: UpStrength(); break;
                case 2: UpDexterity(); break;
                case 3: UpIntelligence(); break;
            }

            Console.WriteLine();
        }

        public void Spell(Hero enemy)
        {
            //Tworzymy 3 Spell'e tak samo jak jest to zrobione w LevelUp()
            Console.Write("  1:FireBall, 2:FrostSpear, 3:Heal ... ");
            int opt = int.Parse(Console.ReadLine());

            switch (opt)
            {
                case 1: FireBallSpell(enemy); break;
                case 2: FrostSpearSpell(enemy); break;
                case 3: HealSpell(); break;
            }

            Console.WriteLine();
        }

        //FireBall da się uniknąć, ale zadaje więcej obrażeń
        public void FireBallSpell(Hero enemy)
        {
            Rand rand = new Rand();
            double damage = Intelligence * rand.Run(5, 10) / 10;

            if (rand.Run(0, 100) > enemy.GetDexterity())
            {
                Console.WriteLine("Bang!");
                enemy.HP -= damage;
            }
            else Console.WriteLine("Dodge!");
        }

        //FrozenSpear nie da się uniknąć, ale zadaje mniej obrażeń
        public void FrostSpearSpell(Hero enemy)
        {
            enemy.HP -= Intelligence / 3;
            Console.WriteLine("Bang!");
        }

        //+5 HP zawsze
        public void HealSpell()
        {
            if (HP < 55)
            {
                HP += 5;
                Console.WriteLine("Regenerated 5 HP.");
            }
            Console.WriteLine("You are fully healed.");
        }

        // TODO: Per-round (regeneration...)
        public void Regeneration()
        {
            if (HP < 55)
            {
                Rand rand = new Rand();
                double regeneration = rand.Run(1, 3);
                HP += regeneration;
                Console.WriteLine("Regenerated " + regeneration + " HP.");
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int tour = 1;

            Hero hero1 = new Hero("Edward Białykij", "sorcerer");
            Console.WriteLine(hero1.Name + " Str:{0} Dex:{1} Int:{2} HP:{3}", hero1.GetStrength(), hero1.GetDexterity(), hero1.GetIntelligence(), hero1.HP);

            Hero hero2 = new Hero("Wataszka Stefan", "assassin");
            Console.WriteLine(hero2.Name + " Str:{0} Dex:{1} Int:{2} HP:{3}", hero2.GetStrength(), hero2.GetDexterity(), hero2.GetIntelligence(), hero2.HP);

            Console.WriteLine();

            while (hero1.HP > 0 && hero2.HP > 0)
            {
                if (tour == 1) Console.WriteLine("Your Turn: " + hero1.Name);
                else Console.WriteLine("Your Turn: " + hero2.Name);

                Console.Write("1:Attack, 2:Spell, 3:LevelUp ... ");
                int opt = int.Parse(Console.ReadLine());

                switch (opt)
                {
                    case 1:
                        if (tour == 1) hero1.Attack(hero2);
                        else hero2.Attack(hero1);
                        break;

                    case 2:
                        //Dodajemy odniesienie do metody, żeby można było używać umiejętności
                        if (tour == 1) hero1.Spell(hero2);
                        else hero2.Spell(hero1);
                        break;

                    case 3:
                        if (tour == 1) hero1.LevelUp();
                        else hero2.LevelUp();
                        break;
                }

                //Regenerujemy wojownika
                if (tour == 1) hero1.Regeneration();
                else hero2.Regeneration();

                Console.WriteLine(hero1.Name + " Str:{0} Dex:{1} Int:{2} HP:{3}", hero1.GetStrength(), hero1.GetDexterity(), hero1.GetIntelligence(), hero1.HP);
                Console.WriteLine(hero2.Name + " Str:{0} Dex:{1} Int:{2} HP:{3}", hero2.GetStrength(), hero2.GetDexterity(), hero2.GetIntelligence(), hero2.HP);
                Console.WriteLine();

                tour++;
                if (tour > 2) tour = 1;
            }

            // TODO: Win
            if (hero1.HP <= 0) Console.WriteLine(hero2.Name + " won!");
            if (hero2.HP <= 0) Console.WriteLine(hero1.Name + " won!");
        }
    }
}