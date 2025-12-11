using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Creatures;

namespace sem3laba3.Spells
{
    public class Booster : Spell
    {
        public Booster() : base(2) { }

        public override void Act(List<Creature> enemy_army)
        {
            foreach(Creature creature in enemy_army)
            {
                creature.IncreaseDamage(Strenght);
            }
        }
    }
}
