using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sem3laba3.Cards
{
    public abstract class Card
    {
        private int _power;
        public int Power
        {
            get { return _power; }
            protected set
            {
                if(value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Некорректное значение мощности");
                }
                _power = value;
            }
        }

        // public abstract void Play();
    }
}
