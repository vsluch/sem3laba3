using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards;
using sem3laba3.Spells;

namespace sem3laba3.Cards.Creatures
{
    public class Knight : Creature
    {
        public Knight() : 
            base(GameBalanceStats.Knight.HP, GameBalanceStats.Knight.Damage, new KnightDamageStrategy()) { }
    }
}
