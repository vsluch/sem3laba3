using sem3laba3.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Creatures;

namespace sem3laba3.Spells
{
    public abstract class Spell : Card
    {
        public int Strenght { get; private set; }

        public Spell(int _strenght)
        {
            if(_strenght >= 1) { Strenght =  _strenght; }
            Power = Strenght * 2;
        }

        public abstract void Act(List<Creature> enemy_army); // играть, действовать
    }
}
