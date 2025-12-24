using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards;
using sem3laba3.Cards.Creatures;

namespace sem3laba3.Spells
{
    public class Booster : Spell
    {
        public Booster() : base(GameBalanceStats.Booster.Strenght) { }

        public override void Act(List<IBattleable> enemy_army)
        {
            foreach(IBattleable unit in enemy_army)
            {
                unit.IncreaseDamage(Strenght);
            }
        }
    }
}
