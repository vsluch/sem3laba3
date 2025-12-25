using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards;
using sem3laba3.Spells;

namespace sem3laba3
{
    public class Hand
    {
        private List<Card> _cards;
        private int _handPower;
        public int HandPower
        {
            get {  return _handPower; }
            private set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException("Некорректное значение мощности руки");
                }
                _handPower = value;
            }
        }

        public int Count => _cards.Count;
        public bool IsEmpty => _cards.Count == 0;


        public Hand()
        {
            _cards = new List<Card>();
            HandPower = 0;
        }

        public void Add(Card card)
        {
            if(card == null)
            {
                throw new ArgumentNullException("Некорректное значение карты");
            }
            _cards.Add(card);
            HandPower += card.Power;
        }

        public Card RemoveAt(int index)
        {
            var card = GetCard(index);
            _cards.RemoveAt(index);
            HandPower -= card.Power;
            return card;
        }

        public Card GetCard(int index)
        {
            if(index < 0 || index >= _cards.Count)
            {
                throw new ArgumentOutOfRangeException("Некорректный индекс карты");
            }
            return _cards[index];
        }

        public List<Card> GetAllCards() => new List<Card>(_cards);
    }
}
