using lab3.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace lab3.Creatures
{
    public class Knight : Creature
    {
        public Knight() : base("Knight", 16, 5) { }
        public override void Hit(IBattleable target)
        {
            int calculatedDamage = this.Damage;

            // Проверяем, является ли цель Ассасином
            if (target is Assassin)
            {
                calculatedDamage += 2; // Бонус
                Console.WriteLine("Рыцарь наносит критический удар по Ассасину!");
            }

            // Наносим итоговый урон
            target.TakeDamage(calculatedDamage);
        }
    }

    public class Wizard : Creature
    {
        public Wizard() : base("Wizard", 8, 6) { }

        public override void Hit(IBattleable target)
        {
            Console.WriteLine($"{Name} атакует {target.Name}!");
            int dmg = Damage;
            if (target is Knight)
            {
                Console.WriteLine("Магия пробивает броню Рыцаря!");
                dmg += 2;
            }
            target.TakeDamage(dmg);
        }
    }

    public class Assassin : Creature
    {
        public Assassin() : base("Assassin", 8, 7) { }

        public override void Hit(IBattleable target)
        {
            Console.WriteLine($"{Name} атакует {target.Name}!");
            int dmg = Damage;
            if (target is Wizard)
            {
                Console.WriteLine("Ассасин застает Мага врасплох!");
                dmg += 2;
            }
            target.TakeDamage(dmg);
        }
    }
}

