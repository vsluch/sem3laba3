using lab3.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    public class Hand
    {
        private List<Card> cards;
        public int HandPower { get; private set; }
        public int CardsCount => cards.Count;

        public Hand()
        {
            cards = new List<Card>();
        }

        public void Add(Card card)
        {
            cards.Add(card);
            HandPower += card.Power;
        }

        public Card GetCardAtIndex(int index)
        {
            if (index >= 0 && index < cards.Count) return cards[index];
            return null;
        }

        public void ThrowAway(Card card)
        {
            if (cards.Contains(card))
            {
                cards.Remove(card);
                HandPower -= card.Power;
            }
        }
    }
}

