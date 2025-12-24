using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sem3laba3.Cards.Creatures
{
    public class CreatureCard : Card    // карта существа
    {
        private Creature _creature;

        public CreatureCard(Creature creature)
        {
            _creature = creature;
            Power = _creature.Damage + _creature.MaxHP;
        }
    }
}
