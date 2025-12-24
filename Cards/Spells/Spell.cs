using sem3laba3.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards.Creatures;

namespace sem3laba3.Spells
{
    public abstract class Spell : Card
    {
        private int _strenght;
        public int Strenght
        {
            get { return _strenght; }
            protected set
            {
                if(value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Некорректное значение силы заклинания");
                }
                _strenght = value;
            }
        }

        public Spell(int strenght)
        {
            Strenght = strenght;
            Power = Strenght * 2;
        }

        public abstract void Act(List<IBattleable> enemy_army); // играть, действовать
    }
}
