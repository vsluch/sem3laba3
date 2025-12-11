using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards;
using sem3laba3.Creatures;
using sem3laba3.Spells;

namespace sem3laba3
{
    public class Hand
    {
        private List<Card> cards;
        public int HandPower { get; private set; }

        public Hand()
        {
            cards = new List<Card>();
            HandPower = 0;
        }

        public void Add(Card card)
        {
            cards.Add(card);
            HandPower += card.Power;
        }

        public Card ThrowAway(Card card)
        {
            if (cards.Contains(card))
            {
                cards.Remove(card);
                HandPower -= card.Power;
                return card;
            }
            return null;    // надо грамотно доделать
        }

    }
}
