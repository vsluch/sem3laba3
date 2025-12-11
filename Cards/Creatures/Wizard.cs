using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sem3laba3.Creatures
{
    public class Wizard : Creature
    {
        public Wizard() : base(8, 6) { }

        public new void Hit(Creature hitCreature)
        {
            if (hitCreature.GetType() == typeof(Knight))
            {
                hitCreature.TakeDamage(Damage + 2);
            }
            else { hitCreature.TakeDamage(Damage); }
        }
    }
}
