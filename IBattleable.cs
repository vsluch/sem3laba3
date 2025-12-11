using sem3laba3.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sem3laba3
{
    public interface IBattleable
    {
        void TakeDamage(int damage);
        void GetHeal(int added_hp);
        void IncreaseDamage(int added_damage);
        void Hit(Creature hitCreature);
    }
}
