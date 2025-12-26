using sem3laba3.Cards;
using sem3laba3.Cards.Creatures;
using sem3laba3.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sem3laba3
{
    public class Market
    {
        private List<Card> _availableCards; // доступные карты
        private readonly int _maxHandPower;

        public Market()
        {
            _availableCards = new List<Card>();
            _maxHandPower = GameBalanceStats.Market.MaxHandPower;
            InitializeMarket();
        }

        private void InitializeMarket()
        {
            // Создаем пул карт для маркета
            for (int i = 0; i < 5; i++)
            {
                _availableCards.Add(new CreatureCard(new Knight()));
                _availableCards.Add(new CreatureCard(new Assassin()));
                _availableCards.Add(new CreatureCard(new Wizard()));
            }

            for (int i = 0; i < 5; i++)
            {
                _availableCards.Add(new Fireball());
                _availableCards.Add(new HealSpell());
                _availableCards.Add(new Booster());
            }
        }

        public List<Card> GetAvailableCards()
        {
            return new List<Card>(_availableCards);
        }

        public void RemoveCard(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException("Некорректная карта");
            }
            _availableCards.Remove(card);
        }

        public void ReturnCard(Card card)
        {
            if (card == null)
                throw new ArgumentNullException("Некорректная карта");

            _availableCards.Add(card);
        }

        public bool CanAddCardToHand(Card card, int currentHandPower)
        {
            if (card == null)
                throw new ArgumentNullException("Некорректная карта");
            return currentHandPower + card.Power <= _maxHandPower;
        }

        public int CalculateRemainingPower(Card card, int currentHandPower)
        {
            if (card == null)
                throw new ArgumentNullException("Некорректная карта");
            return _maxHandPower - (currentHandPower + card.Power);
        }
    }
}