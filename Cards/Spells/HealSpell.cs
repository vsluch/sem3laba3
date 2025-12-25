using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards;
using sem3laba3.Cards.Creatures;
using sem3laba3.Cards.Spells;

namespace sem3laba3.Spells
{
    public class HealSpell : Spell
    {
        public HealSpell() : base(GameBalanceStats.HeallSpell.Strenght) { }

        public override void Act(List<IBattleable> enemy_army)
        {
            foreach (IBattleable unit in enemy_army)
            {
                unit.GetHeal(Strenght);
            }
        }
    }
}
