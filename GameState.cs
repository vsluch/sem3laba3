using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards;
using sem3laba3.Cards.Creatures;
using sem3laba3.Spells;
using Newtonsoft.Json;

namespace sem3laba3
{
    [Serializable]
    public class GameState
    {
        public PlayerState Player1State { get; set; }
        public PlayerState Player2State { get; set; }
        public BoardState BoardState { get; set; }
        public int CurrentPlayerNumber { get; set; }
        public int TurnNumber { get; set; }
        public bool GameStarted { get; set; }

        [JsonConstructor]
        public GameState() { }

        public GameState(Player player1, Player player2, Board board, int currentPlayer, int turnNumber, bool gameStarted)
        {
            Player1State = new PlayerState(player1);
            Player2State = new PlayerState(player2);
            BoardState = new BoardState(board);
            CurrentPlayerNumber = currentPlayer;
            TurnNumber = turnNumber;
            GameStarted = gameStarted;
        }
    }

    [Serializable]
    public class PlayerState
    {
        public int PlayerNumber { get; set; }
        public List<CardInfo> Hand { get; set; }
        public int ActionsRemaining { get; set; }

        public PlayerState(Player player)
        {
            PlayerNumber = player.PlayerNumber;
            ActionsRemaining = player.ActionsRemaining;
            Hand = new List<CardInfo>();

            // Сохраняем информацию о картах в руке
            for (int i = 0; i < player.GetHandCount(); i++)
            {
                var card = player.PlayerHand.GetCard(i);
                Hand.Add(new CardInfo(card));
            }
        }
    }

    [Serializable]
    public class CardInfo
    {
        public string Type { get; set; }
        public int Power { get; set; }
        public string CreatureType { get; set; }
        public int Damage { get; set; }
        public int HP { get; set; }
        public int SpellStrength { get; set; }

        public CardInfo(Card card)
        {
            Power = card.Power;

            if (card is CreatureCard creatureCard)
            {
                Type = "Creature";
                var creature = creatureCard.CreateCreature();
                CreatureType = creature.GetType().Name;
                Damage = creature.GetDamage();
                HP = creature.HP;
            }
            else if (card is Fireball)
            {
                Type = "Fireball";
                SpellStrength = GameBalanceStats.Fireball.Strenght;
            }
            else if (card is HealSpell)
            {
                Type = "HealSpell";
                SpellStrength = GameBalanceStats.HealSpell.Strenght;
            }
            else if (card is Booster)
            {
                Type = "Booster";
                SpellStrength = GameBalanceStats.Booster.Strenght;
            }
            else
            {
                Type = "Unknown";
            }
        }
    }

    [Serializable]
    public class BoardState
    {
        public List<CreatureInfo> Player1Army { get; set; }
        public List<CreatureInfo> Player2Army { get; set; }

        public BoardState(Board board)
        {
            Player1Army = new List<CreatureInfo>();
            Player2Army = new List<CreatureInfo>();

            foreach (var unit in board.Player1Army)
            {
                if (unit is Creature creature)
                {
                    Player1Army.Add(new CreatureInfo(creature));
                }
            }

            foreach (var unit in board.Player2Army)
            {
                if (unit is Creature creature)
                {
                    Player2Army.Add(new CreatureInfo(creature));
                }
            }
        }
    }

    [Serializable]
    public class CreatureInfo
    {
        public string Type { get; set; }
        public int Damage { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }

        public CreatureInfo(Creature creature)
        {
            Type = creature.GetType().Name;
            Damage = creature.GetDamage();
            HP = creature.HP;
            MaxHP = creature.MaxHP;
        }
    }
}