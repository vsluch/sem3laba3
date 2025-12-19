using lab3;
using lab3.Cards;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace lab3.Creatures
{
    public abstract class Creature : Card, IBattleable
    {
        public int Damage { get; protected set; }
        public int HP { get; protected set; }
        public int MaxHP { get; protected set; }

        //Существо мертво, если его здоровье меньше или равно 0
        public bool IsDead => HP <= 0;

        public Creature(string name, int hp, int damage)
        {
            Name = name;
            if (damage >= 1) Damage = damage;
            if (hp >= 1)
            {
                HP = hp;
                MaxHP = hp;
            }
            Power = HP + Damage;
        }

        //Реализация Play (выставляет карту на стол)
        public override void Play(List<Creature> ownerArmy, List<Creature> enemyArmy)
        {
            ownerArmy.Add(this);
            Console.WriteLine($"{Name} вступает в бой!");
        }

        public void TakeDamage(int damageTaken)
        {
            HP -= damageTaken;
            if (HP < 0) HP = 0;
            Console.WriteLine($"{Name} получает {damageTaken} урона. Осталось HP: {HP}");
        }

        public void GetHeal(int added_hp)
        {
            HP += added_hp;
            if (HP > MaxHP) HP = MaxHP;
        }

        public void IncreaseDamage(int added_damage)
        {
            Damage += added_damage;
        }

        public virtual void Hit(IBattleable target)
        {
            if (target != null && !target.IsDead)
            {
                target.TakeDamage(Damage);
            }
        }
    }
}
