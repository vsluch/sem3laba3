using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sem3laba3
{
    public class Game
    {
        private Player Player1;
        private Player Player2;
        private Board GameBoard;
        private Player ActivePlayer;

        public void Start()
        {
            GameBoard = new Board();

            Player1 = new Player(1);
            Player2 = new Player(2);
            ActivePlayer = Player1;
        }


        public void PlayerPlaysCreatureCard(int playerNumber, int indexInHand)  // игрок разыгрывает карту существа
        {
            if (playerNumber != 1 && playerNumber != 2)
            {
                throw new ArgumentOutOfRangeException("Некорректный номер игрока");
            }

            var player = playerNumber == 1 ? Player1 : Player2;
            if(player != ActivePlayer)
            {
                throw new InvalidOperationException("Сейчас ход другого игрока");
            }

            var creature = player.PlayCreatureCard(indexInHand);
            GameBoard.AddUnit(playerNumber, creature);

            if(!player.HasActions) { EndTurn(); }
        }

        private void EndTurn()
        {
            if(ActivePlayer == Player1)
            {
                ActivePlayer = Player2;
            }
            else
            {
                ActivePlayer = Player1;
            }
            ActivePlayer.StartTurn();
        }


        public void PlayerAttacks(int playerNumber, int attackerIndex, IBattleable target)
        {
            var player = playerNumber == 1 ? Player1 : Player2;
            var attacker = GameBoard.GetPlayerArmy(playerNumber)[attackerIndex];  // корректность номера игрока проверится там
            if (player != ActivePlayer)
            {
                throw new InvalidOperationException("Сейчас ход другого игрока");
            }
            attacker.Hit(target);

            GameBoard.RemoveDeadCreatures();
            player.UseAction();

            if (!player.HasActions) { EndTurn(); }
        }

        
        public bool PlayerHasLost(int playerNumber)
        {
            if (playerNumber != 1 && playerNumber != 2)
            {
                throw new ArgumentOutOfRangeException("Некорректный номер игрока");
            }
            var player = playerNumber == 1 ? Player1 : Player2;

            if(!player.HasCardsInHand() && GameBoard.GetPlayerArmy(playerNumber).Count == 0)
            {
                return true;
            }
            return false;
        }

        public bool IsGameOver()
        {
            return PlayerHasLost(1) || PlayerHasLost(2);
        }
        public int? GetWinner()
        {
            if (PlayerHasLost(1) && !PlayerHasLost(2)) return 2;
            if (PlayerHasLost(2) && !PlayerHasLost(1)) return 1;
            return null;
        }



    }
}
