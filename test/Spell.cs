using lab3.Cards;
using lab3.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace lab3.Spells
{
    public abstract class Spell : Card
    {
        public int Strength { get; private set; }

        public Spell(string name, int strength)
        {
            Name = name;
            Strength = strength;
            Power = Strength * 2;
        }
        // Act убрал, так как теперь есть общий Play в классе Card
    }

    public class Fireball : Spell
    {
        public Fireball() : base("Fireball", 3) { }

        public override void Play(List<Creature> ownerArmy, List<Creature> enemyArmy)
        {
            Console.WriteLine($"Кастуется {Name}! Враги горят!");
            // Fireball бьет ВРАГОВ (enemyArmy)
            foreach (var c in enemyArmy) c.TakeDamage(Strength);
        }
    }

    public class HealSpell : Spell
    {
        public HealSpell() : base("Heal", 4) { }

        public override void Play(List<Creature> ownerArmy, List<Creature> enemyArmy)
        {
            Console.WriteLine($"Кастуется {Name}! Союзники лечатся!");
            // Лечит СВОИХ (ownerArmy)
            foreach (var c in ownerArmy) c.GetHeal(Strength);
        }
    }

    public class Booster : Spell
    {
        public Booster() : base("Booster", 2) { }

        public override void Play(List<Creature> ownerArmy, List<Creature> enemyArmy)
        {
            Console.WriteLine($"Кастуется {Name}! Атака союзников растет!");
            // Бафает СВОИХ (ownerArmy)
            foreach (var c in ownerArmy) c.IncreaseDamage(Strength);
        }
    }
}
