using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards;
using sem3laba3.Cards.Creatures;
using sem3laba3.Cards.Spells;

namespace sem3laba3
{
    public class Player
    {
        public Hand PlayerHand { get; }
            

        private int _actionsRemaining;
        public int ActionsRemaining
        {
            get { return _actionsRemaining; }
            set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException("Некорректное значение оставшихся действий");
                }
                _actionsRemaining = value;
            }
        }
        public bool HasActions => ActionsRemaining > 0;

        private int _playerNumber;
        public int PlayerNumber
        {
            get { return _playerNumber; }
            set
            {
                if(value != 1 && value != 2)
                {
                    throw new ArgumentOutOfRangeException("Некорректное значение номера игрока");
                }
                _playerNumber = value;
            }
        }

        public Player(int playerNumber)
        {
            PlayerNumber = playerNumber;
            PlayerHand = new Hand();
            ActionsRemaining = 0;
        }

        public void StartTurn()
        {
            ActionsRemaining = 2;
        }
        public void UseAction()
        {
            if(ActionsRemaining <= 0)
            {
                throw new InvalidOperationException("Нет доступных действий");
            }
            ActionsRemaining--;
        }


        public Creature PlayCreatureCard(int indexInHand)   // разместить существо на поле
        {
            UseAction();
            var card = PlayerHand.GetCard(indexInHand);

            if(card is CreatureCard creatureCard)
            {
                PlayerHand.RemoveAt(indexInHand);
                return creatureCard.CreateCreature();
            }
            throw new InvalidOperationException("Это не карта существа");
        }

        public void PlaySpellCard(int indexInHand, List<IBattleable> targets)   // разыграть заклинание
        {
            UseAction();
            var card = PlayerHand.GetCard(indexInHand);

            if(card is Spell spell)
            {
                spell.Act(targets);
                PlayerHand.RemoveAt(indexInHand);
            }
            else
            {
                throw new InvalidOperationException("Это не заклинание");
            }
        }

        public void AddCardToHand(Card card)
        {
            PlayerHand.Add(card);
        }

        public int GetHandCount()
        {
            return PlayerHand.Count;
        }

        public bool HasCardsInHand()
        {
            return !PlayerHand.IsEmpty;
        }

        public int GetHandPower()
        {
            return PlayerHand.HandPower;
        }


        
    }
}
