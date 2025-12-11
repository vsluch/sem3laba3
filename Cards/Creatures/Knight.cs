using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards;
using sem3laba3.Spells;

namespace sem3laba3.Creatures
{
    public class Knight : Creature
    {
        public Knight() : base(16, 5) { }

        public new void Hit(Creature hitCreature)
        {
            if(hitCreature.GetType() == typeof(Assassin))
            {
                hitCreature.TakeDamage(Damage + 2);
            }
            else { hitCreature.TakeDamage(Damage); }
        }
    }
}
