using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sem3laba3.Cards;
using sem3laba3.Cards.Creatures;
using sem3laba3.Spells;

namespace sem3laba3
{
    public class Game
    {
        private Player _player1;
        private Player _player2;
        private Board _gameBoard;
        private Player _activePlayer;
        private int _turnNumber;
        private bool _gameStarted;

        public Player Player1 => _player1;
        public Player Player2 => _player2;
        public Board Board => _gameBoard;
        public Player ActivePlayer => _activePlayer;
        public int TurnNumber => _turnNumber;
        public bool GameStarted => _gameStarted;

        public Game()
        {
            _gameBoard = new Board();
            _turnNumber = 1;
            _gameStarted = false;
        }

        // Конструктор для загрузки из сохранения
        public Game(Player player1, Player player2, Board board, Player activePlayer, int turnNumber, bool gameStarted)
        {
            _player1 = player1;
            _player2 = player2;
            _gameBoard = board;
            _activePlayer = activePlayer;
            _turnNumber = turnNumber;
            _gameStarted = gameStarted;
        }

        public void Start()
        {
            _player1 = new Player(1);
            _player2 = new Player(2);
            _activePlayer = _player1;

            // Тестовые карты для отладки
            _player1.AddCardToHand(new CreatureCard(new Knight()));
            _player1.AddCardToHand(new CreatureCard(new Assassin()));
            _player1.AddCardToHand(new Fireball());

            _player2.AddCardToHand(new CreatureCard(new Wizard()));
            _player2.AddCardToHand(new CreatureCard(new Assassin()));
            _player2.AddCardToHand(new HealSpell());

            _activePlayer.StartTurn();
            _gameStarted = true;
        }

        // Остальные методы оставляем без изменений...
        public void PlayerPlaysCreatureCard(int playerNumber, int indexInHand)
        {
            if (!_gameStarted)
            {
                throw new InvalidOperationException("Игра еще не начата!");
            }

            if (playerNumber != 1 && playerNumber != 2)
            {
                throw new ArgumentOutOfRangeException("Некорректный номер игрока");
            }

            var player = playerNumber == 1 ? _player1 : _player2;
            if (player != _activePlayer)
            {
                throw new InvalidOperationException("Сейчас ход другого игрока");
            }

            var creature = player.PlayCreatureCard(indexInHand);
            _gameBoard.AddUnit(playerNumber, creature);

            if (!player.HasActions) { EndTurn(); }
        }

        public void PlayerPlaysSpell(int playerNumber, int indexInHand, List<IBattleable> targets)
        {
            if (!_gameStarted)
            {
                throw new InvalidOperationException("Игра еще не начата!");
            }

            if (playerNumber != 1 && playerNumber != 2)
            {
                throw new ArgumentOutOfRangeException("Некорректный номер игрока");
            }

            var player = playerNumber == 1 ? _player1 : _player2;
            if (player != _activePlayer)
            {
                throw new InvalidOperationException("Сейчас ход другого игрока");
            }

            player.PlaySpellCard(indexInHand, targets);
            _gameBoard.RemoveDeadCreatures();

            if (!player.HasActions) { EndTurn(); }
        }

        public void EndTurn()
        {
            if (_activePlayer == _player1)
            {
                _activePlayer = _player2;
            }
            else
            {
                _activePlayer = _player1;
            }
            _activePlayer.StartTurn();
            _turnNumber++;
        }

        public void PlayerAttacks(int playerNumber, int attackerIndex, IBattleable target)
        {
            if (!_gameStarted)
            {
                throw new InvalidOperationException("Игра еще не начата!");
            }

            var player = playerNumber == 1 ? _player1 : _player2;
            var attackerArmy = _gameBoard.GetPlayerArmy(playerNumber);

            if (attackerIndex < 0 || attackerIndex >= attackerArmy.Count)
            {
                throw new ArgumentOutOfRangeException("Некорректный индекс атакующего");
            }

            var attacker = attackerArmy[attackerIndex];

            if (player != _activePlayer)
            {
                throw new InvalidOperationException("Сейчас ход другого игрока");
            }

            attacker.Hit(target);
            _gameBoard.RemoveDeadCreatures();
            player.UseAction();

            if (!player.HasActions) { EndTurn(); }
        }

        public static Game LoadFromState(GameState gameState)
        {
            try
            {
                // Восстанавливаем игроков
                Player player1 = ReconstructPlayer(gameState.Player1State);
                Player player2 = ReconstructPlayer(gameState.Player2State);

                // Восстанавливаем доску
                Board board = ReconstructBoard(gameState.BoardState);

                // Определяем активного игрока
                Player activePlayer = gameState.CurrentPlayerNumber == 1 ? player1 : player2;

                // Создаем игру
                Game game = new Game(player1, player2, board, activePlayer,
                    gameState.TurnNumber, gameState.GameStarted);

                return game;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при загрузке игры: {ex.Message}");
            }
        }

        private static Player ReconstructPlayer(PlayerState playerState)
        {
            Player player = new Player(playerState.PlayerNumber);
            player.ActionsRemaining = playerState.ActionsRemaining;

            // Восстанавливаем карты в руке
            foreach (var cardInfo in playerState.Hand)
            {
                Card card = ReconstructCard(cardInfo);
                player.AddCardToHand(card);
            }

            return player;
        }

        private static Card ReconstructCard(CardInfo cardInfo)
        {
            switch (cardInfo.Type)
            {
                case "Creature":
                    Creature creature = null;
                    switch (cardInfo.CreatureType)
                    {
                        case "Knight":
                            creature = new Knight();
                            break;
                        case "Assassin":
                            creature = new Assassin();
                            break;
                        case "Wizard":
                            creature = new Wizard();
                            break;
                    }
                    if (creature != null)
                    {
                        // Восстанавливаем состояние существа
                        creature.IncreaseDamage(cardInfo.Damage - creature.GetDamage());
                        creature.GetHeal(cardInfo.HP - creature.HP);
                        return new CreatureCard(creature);
                    }
                    break;

                case "Fireball":
                    return new Fireball();

                case "HealSpell":
                    return new HealSpell();

                case "Booster":
                    return new Booster();
            }

            throw new Exception($"Неизвестный тип карты: {cardInfo.Type}");
        }

        private static Board ReconstructBoard(BoardState boardState)
        {
            Board board = new Board();

            // Восстанавливаем армию игрока 1
            foreach (var creatureInfo in boardState.Player1Army)
            {
                IBattleable creature = ReconstructCreature(creatureInfo);
                board.AddUnit(1, creature);
            }

            // Восстанавливаем армию игрока 2
            foreach (var creatureInfo in boardState.Player2Army)
            {
                IBattleable creature = ReconstructCreature(creatureInfo);
                board.AddUnit(2, creature);
            }

            return board;
        }

        private static IBattleable ReconstructCreature(CreatureInfo creatureInfo)
        {
            Creature creature = null;

            switch (creatureInfo.Type)
            {
                case "Knight":
                    creature = new Knight();
                    break;
                case "Assassin":
                    creature = new Assassin();
                    break;
                case "Wizard":
                    creature = new Wizard();
                    break;
            }

            if (creature != null)
            {
                // Восстанавливаем состояние существа
                creature.IncreaseDamage(creatureInfo.Damage - creature.GetDamage());
                creature.GetHeal(creatureInfo.HP - creature.HP);
            }

            return creature;
        }

        public bool PlayerHasLost(int playerNumber)
        {
            if (playerNumber != 1 && playerNumber != 2)
            {
                throw new ArgumentOutOfRangeException("Некорректный номер игрока");
            }

            var player = playerNumber == 1 ? _player1 : _player2;
            return !player.HasCardsInHand() && _gameBoard.GetPlayerArmy(playerNumber).Count == 0;
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

        public void StartWithPlayers(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;
            _activePlayer = _player1;
            _activePlayer.StartTurn();
            _gameStarted = true;
        }


        public void StartGame()
        {
            if (_player1 == null || _player2 == null)
            {
                throw new InvalidOperationException("Игроки не инициализированы!");
            }

            _activePlayer = _player1;
            _activePlayer.StartTurn();
            _gameStarted = true;
        }



        // Методы для сохранения и загрузки
        public void SaveGame(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentException("Путь к файлу не может быть пустым");
                }

                if (!_gameStarted)
                {
                    throw new InvalidOperationException("Нельзя сохранить игру, которая не начата");
                }

                // Создаем директорию, если ее нет
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                GameState gameState = new GameState(_player1, _player2, _gameBoard,
                    _activePlayer.PlayerNumber, _turnNumber, _gameStarted);

                string json = JsonConvert.SerializeObject(gameState,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        NullValueHandling = NullValueHandling.Ignore
                    });

                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сохранении игры: {ex.Message}", ex);
            }
        }

        public static Game LoadGame(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("Файл сохранения не найден");
                }

                string json = File.ReadAllText(filePath);
                GameState gameState = JsonConvert.DeserializeObject<GameState>(json);

                if (gameState == null)
                {
                    throw new Exception("Неверный формат файла сохранения");
                }

                return LoadFromState(gameState);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при загрузке игры: {ex.Message}", ex);
            }
        }
    }
}