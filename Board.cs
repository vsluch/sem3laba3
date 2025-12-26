using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sem3laba3.Cards.Creatures;

namespace sem3laba3
{
    public class Board
    {
        public List<IBattleable> Player1Army { get; }
        public List<IBattleable> Player2Army { get; }

        public Board()
        {
            Player1Army = new List<IBattleable>();
            Player2Army = new List<IBattleable>();
        }

        public List<IBattleable> GetPlayerArmy(int playerNumber)
        {
            CorrectPlayerNumber(playerNumber);
            return playerNumber == 1 ? Player1Army : Player2Army;
        }

        public List<IBattleable> GetOpponentArmy(int playerNumber)
        {
            CorrectPlayerNumber(playerNumber);
            return playerNumber == 1 ? Player2Army : Player1Army;
        }

        public void AddUnit(int playerNumber, IBattleable unit)
        {
            if(unit == null)
            {
                throw new ArgumentNullException("Некорректный юнит");
            }
            CorrectPlayerNumber(playerNumber);

            if (playerNumber == 1) { Player1Army.Add(unit); }
            else { Player2Army.Add(unit); }
        }

        public void RemoveDeadCreatures()   // Удаление мертвых
        {
            var deadCreatures1 = Player1Army.Where(c => c is Creature creature && creature.HP <= 0).ToList();
            var deadCreatures2 = Player2Army.Where(c => c is Creature creature && creature.HP <= 0).ToList();

            foreach (var creature in deadCreatures1)
            {
                Player1Army.Remove(creature);
            }
            foreach (var creature in deadCreatures2)
            {
                Player2Army.Remove(creature);
            }
        }

        public bool HasArmy(int playerNumber)
        {
            CorrectPlayerNumber(playerNumber);
            return GetPlayerArmy(playerNumber).Count > 0;
        }



        public void CorrectPlayerNumber(int playerNumber)
        {
            if (playerNumber != 1 && playerNumber != 2)
            {
                throw new ArgumentOutOfRangeException("Некорректный номер игрока");
            }
        }
    }
}
