using lab3.Cards;
using lab3.Creatures;
using lab3.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    public class Player
    {
        public string Name { get; private set; }
        public int HP { get; set; } = 30;
        public Hand Hand { get; private set; }
        public List<Creature> Army { get; private set; } // Существа на столе
        private List<Card> Deck;

        public Player(string name)
        {
            Name = name;
            Hand = new Hand();
            Army = new List<Creature>();
            Deck = GenerateDeck();
        }

        private List<Card> GenerateDeck()
        {
            List<Card> newDeck = new List<Card>();
            // Заполним случайными картами
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                int r = rnd.Next(0, 6);
                switch (r)
                {
                    case 0: newDeck.Add(new Knight()); break;
                    case 1: newDeck.Add(new Wizard()); break;
                    case 2: newDeck.Add(new Assassin()); break;
                    case 3: newDeck.Add(new Fireball()); break;
                    case 4: newDeck.Add(new HealSpell()); break;
                    case 5: newDeck.Add(new Booster()); break;
                }
            }
            return newDeck;
        }

        public void DrawCard()
        {
            if (Deck.Count > 0)
            {
                Card c = Deck[0];
                Deck.RemoveAt(0);
                Hand.Add(c);
                Console.WriteLine($"{Name} берет карту: {c.Name}");
            }
        }

        public void MakeMove(List<Creature> enemyArmy)
        {
            Console.WriteLine($"\n--- Ход игрока {Name} (HP: {HP}) ---");
            DrawCard();

            // 1. Атака существами на столе
            if (Army.Count > 0 && enemyArmy.Count > 0)
            {
                Console.WriteLine("Фаза атаки существ:");
                // Упрощенная логика: каждое наше существо бьет первое попавшееся вражеское
                foreach (var myCreature in Army)
                {
                    if (enemyArmy.Count > 0)
                    {
                        var target = enemyArmy[0];
                        myCreature.Hit(target);
                        if (target.IsDead)
                        {
                            Console.WriteLine($"{target.Name} погиб!");
                            enemyArmy.Remove(target);
                        }
                    }
                }
            }

            // 2. Розыгрыш карт из руки (играем первую попавшуюся для примера)
            if (Hand.CardsCount > 0)
            {
                Card cardToPlay = Hand.GetCardAtIndex(0);
                Console.WriteLine($"Игрок разыгрывает карту: {cardToPlay.Name}");

                // Самый важный момент:
                cardToPlay.Play(this.Army, enemyArmy);

                Hand.ThrowAway(cardToPlay);
            }
        }
    }
}
