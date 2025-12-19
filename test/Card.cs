using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab3.Creatures;

namespace lab3.Cards
{
    public abstract class Card
    {
        public string Name { get; protected set; }
        public int Power { get; protected set; }

        // Метод розыгрыша карты. 
        // ownerArmy - армия игрока, enemyArmy - армия противника
        public abstract void Play(List<Creature> ownerArmy, List<Creature> enemyArmy);

        public override string ToString()
        {
            return $"{Name} (Power: {Power})";
        }
    }
}
