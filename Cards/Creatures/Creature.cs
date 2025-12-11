using sem3laba3.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards;
using sem3laba3.Spells;


namespace sem3laba3.Creatures
{
    public abstract class Creature : Card, IBattleable
    {
        public int Damage { get; private set; }
        public int HP { get; private set; }
        public int MaxHP { get; private set; }
        
        public Creature(int _hp, int _damage)
        {
            if(_damage >= 1 && _damage <= 10) { Damage = _damage; }
            if(_hp >= 1 && _hp <= 20)
            {
                HP = _hp;
                MaxHP = _hp;
            }
            Power = HP + Damage;
        }

        public void TakeDamage(int damageTaken)
        {
            HP -= damageTaken;
            if(HP < 0) HP = 0;
        }

        public virtual void Hit(Creature hitCreature)
        {
            hitCreature.TakeDamage(Damage);
        }

        public void GetHeal(int added_hp)
        {
            HP += added_hp;
            if(HP > MaxHP) {  HP = MaxHP; }
        }

        public void IncreaseDamage(int added_damage)
        {
            Damage += added_damage;
        }




        // для тестирования
        public override string ToString()
        {
            return $"D: {Damage}, HP: {HP}, MHP: {MaxHP}, Power: {Power}";
        }
    }
}
