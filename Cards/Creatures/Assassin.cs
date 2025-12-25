using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards;
using sem3laba3.Spells;

namespace sem3laba3.Cards.Creatures
{
    public class Assassin : Creature
    {
        public Assassin() : 
            base(GameBalanceStats.Assassin.HP, GameBalanceStats.Assassin.Damage, new AssassinDamageStrategy()) { }

        public override Creature Clone()
        {
            return new Assassin();
        }
    }
}
