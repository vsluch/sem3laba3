using sem3laba3.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards;
using sem3laba3.Spells;


namespace sem3laba3.Cards.Creatures
{
    public abstract class Creature : IBattleable    // боевое существа на столе, не карта
    {
        private int _damage;
        public int Damage
        {
            get {  return _damage; }
            private set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException("Некорректное значение урона");
                }
                _damage = value;
            }
        }

        private int _hp;
        public int HP
        {
            get { return _hp; }
            private set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException("Некорректное значение очков здоровья");
                }
                _hp = value;
            }
        }

        private int _maxhp;
        public int MaxHP
        {
            get { return _maxhp; }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Некорректное значение очков полного здоровья");
                }
                _maxhp = value;
            }
        }

        private IDamageStrategy _damageStrategy;

        
        public Creature(int hp, int damage, IDamageStrategy damageStrategy)
        {
            HP = hp;
            Damage = damage;
            MaxHP = HP;
            _damageStrategy = damageStrategy;
        }

        public void TakeDamage(int damageTaken)
        {
            HP -= damageTaken;
            if(HP < 0) HP = 0;
        }

        public virtual void Hit(IBattleable target)
        {
            if(target is Creature creatureTarget)
            {
                int damage = _damageStrategy.CalculateDamage(this, creatureTarget);
                creatureTarget.TakeDamage(damage);
            }
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

        public int GetDamage() { return Damage; }



        // для тестирования
        public override string ToString()
        {
            return $"D: {Damage}, HP: {HP}, MHP: {MaxHP}";
        }
    }
}
