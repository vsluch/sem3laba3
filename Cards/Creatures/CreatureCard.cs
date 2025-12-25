using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sem3laba3.Cards.Creatures
{
    public class CreatureCard : Card    // карта существа
    {
        private Creature _prototype;

        public CreatureCard(Creature prototype)
        {
            _prototype = prototype;
            Power = _prototype.Damage + _prototype.MaxHP;
        }

        public Creature CreateCreature()
        {
            return _prototype.Clone();
        }
    }
}
