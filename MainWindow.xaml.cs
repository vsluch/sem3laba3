using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using sem3laba3.Cards;
using sem3laba3.Cards.Creatures;
using sem3laba3.Cards.Spells;
using sem3laba3.Spells;

namespace sem3laba3
{
    public partial class MainWindow : Window
    {
        private Game _game;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации окна: {ex.Message}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        public MainWindow(Game game) : this()
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            _game = game;
            UpdateGameUI();
            AddLog("Игра загружена из сохранения!");
            UpdateButtonStates();

            if (_game.GameStarted)
            {
                TurnIndicator.Text = $"Ход игрока {_game.ActivePlayer.PlayerNumber}";
                AddLog($"Ход игрока {_game.ActivePlayer.PlayerNumber}");
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _game = new Game();
                _game.Start();

                UpdateGameUI();
                AddLog("Игра начата! Ход игрока 1");
                TurnIndicator.Text = "Игра начата! Ход игрока 1";
            }
            catch (Exception ex)
            {
                TurnIndicator.Text = $"Ошибка: {ex.Message}";
                AddLog($"Ошибка: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_game == null)
            {
                AddLog("Нет активной игры для сохранения!");
                return;
            }

            try
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Файлы сохранения игры (*.json)|*.json|Все файлы (*.*)|*.*",
                    Title = "Сохранить игру",
                    DefaultExt = "json",
                    AddExtension = true
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    _game.SaveGame(saveFileDialog.FileName);
                    AddLog($"Игра сохранена в файл: {saveFileDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PlayCardButton1_Click(object sender, RoutedEventArgs e)
        {
            PlayCard(1);
        }

        private void PlayCardButton2_Click(object sender, RoutedEventArgs e)
        {
            PlayCard(2);
        }

        private void PlayCard(int playerNumber)
        {
            if (_game == null)
            {
                AddLog("Сначала начните игру!");
                return;
            }

            try
            {
                if (_game.ActivePlayer.PlayerNumber != playerNumber)
                {
                    AddLog($"Сейчас не ваш ход! Ход игрока {_game.ActivePlayer.PlayerNumber}.");
                    return;
                }

                // Получаем выбранную карту
                int selectedIndex = playerNumber == 1 ?
                    Player1HandList.SelectedIndex : Player2HandList.SelectedIndex;

                if (selectedIndex == -1)
                {
                    AddLog($"Игрок {playerNumber}: выберите карту для розыгрыша!");
                    return;
                }

                _game.PlayerPlaysCreatureCard(playerNumber, selectedIndex);
                AddLog($"Игрок {playerNumber} разыграл карту существа");

                UpdateGameUI();
                ClearSelections();
            }
            catch (InvalidOperationException ex)
            {
                AddLog($"Не удалось разыграть карту: {ex.Message}");
            }
            catch (Exception ex)
            {
                AddLog($"Ошибка: {ex.Message}");
            }
        }

        private void SkipTurnButton1_Click(object sender, RoutedEventArgs e)
        {
            SkipTurn(1);
        }

        private void SkipTurnButton2_Click(object sender, RoutedEventArgs e)
        {
            SkipTurn(2);
        }

        private void SkipTurn(int playerNumber)
        {
            if (_game == null)
            {
                AddLog("Сначала начните игру!");
                return;
            }

            try
            {
                if (_game.ActivePlayer.PlayerNumber != playerNumber)
                {
                    AddLog($"Сейчас не ваш ход! Ход игрока {_game.ActivePlayer.PlayerNumber}.");
                    return;
                }

                var player = playerNumber == 1 ? _game.Player1 : _game.Player2;

                // Используем одно действие для пропуска
                player.UseAction();
                AddLog($"Игрок {playerNumber} пропускает действие. Осталось действий: {player.ActionsRemaining}");

                // Если у игрока закончились действия, переходим к следующему ходу
                if (!player.HasActions)
                {
                    _game.EndTurn();
                    AddLog($"Ход переходит к игроку {_game.ActivePlayer.PlayerNumber}");
                }

                UpdateGameUI();
            }
            catch (Exception ex)
            {
                AddLog($"Ошибка при пропуске хода: {ex.Message}");
            }
        }

        private void AttackButton1_Click(object sender, RoutedEventArgs e)
        {
            Attack(1, 2);
        }

        private void AttackButton2_Click(object sender, RoutedEventArgs e)
        {
            Attack(2, 1);
        }

        private void Attack(int attackerNumber, int targetPlayerNumber)
        {
            if (_game == null)
            {
                AddLog("Сначала начните игру!");
                return;
            }

            try
            {
                if (_game.ActivePlayer.PlayerNumber != attackerNumber)
                {
                    AddLog($"Сейчас не ваш ход! Ход игрока {_game.ActivePlayer.PlayerNumber}.");
                    return;
                }

                // Получаем выбранное атакующее существо
                int attackerIndex = attackerNumber == 1 ?
                    Player1CreaturesList.SelectedIndex : Player2CreaturesList.SelectedIndex;

                // Получаем выбранную цель
                int targetIndex = targetPlayerNumber == 1 ?
                    Player1CreaturesList.SelectedIndex : Player2CreaturesList.SelectedIndex;

                // Если не выбрано атакующее существо, берем первое
                if (attackerIndex == -1)
                {
                    var attackerArmy = _game.Board.GetPlayerArmy(attackerNumber);
                    if (attackerArmy.Count == 0)
                    {
                        AddLog($"У игрока {attackerNumber} нет существ для атаки!");
                        return;
                    }
                    attackerIndex = 0;
                }

                // Получаем армию противника
                var targetArmy = _game.Board.GetPlayerArmy(targetPlayerNumber);

                if (targetArmy.Count == 0)
                {
                    AddLog($"У игрока {targetPlayerNumber} нет существ для атаки!");
                    return;
                }

                // Если не выбрана цель, берем первое существо
                if (targetIndex == -1)
                {
                    targetIndex = 0;
                }
                else if (targetIndex >= targetArmy.Count)
                {
                    targetIndex = 0; // Если выбранный индекс вне диапазона, берем первое
                }

                // Выполняем атаку
                _game.PlayerAttacks(attackerNumber, attackerIndex, targetArmy[targetIndex]);
                AddLog($"Игрок {attackerNumber} атаковал существо игрока {targetPlayerNumber}");

                UpdateGameUI();
                ClearSelections();
            }
            catch (Exception ex)
            {
                AddLog($"Ошибка атаки: {ex.Message}");
            }
        }

        private void PlaySpellButton1_Click(object sender, RoutedEventArgs e)
        {
            PlaySpell(1);
        }

        private void PlaySpellButton2_Click(object sender, RoutedEventArgs e)
        {
            PlaySpell(2);
        }

        private void PlaySpell(int playerNumber)
        {
            if (_game == null)
            {
                AddLog("Сначала начните игру!");
                return;
            }

            try
            {
                if (_game.ActivePlayer.PlayerNumber != playerNumber)
                {
                    AddLog($"Сейчас не ваш ход! Ход игрока {_game.ActivePlayer.PlayerNumber}.");
                    return;
                }

                // Получаем выбранную карту
                int selectedIndex = playerNumber == 1 ?
                    Player1HandList.SelectedIndex : Player2HandList.SelectedIndex;

                if (selectedIndex == -1)
                {
                    AddLog($"Игрок {playerNumber}: выберите заклинание для розыгрыша!");
                    return;
                }

                var player = playerNumber == 1 ? _game.Player1 : _game.Player2;
                var card = player.PlayerHand.GetCard(selectedIndex);

                if (card is Spell spell)
                {
                    // Определяем цели для заклинания
                    List<IBattleable> targets = new List<IBattleable>();

                    if (spell is Fireball)
                    {
                        // Огненный шар наносит урон армии противника
                        targets = _game.Board.GetOpponentArmy(playerNumber);
                        AddLog($"Игрок {playerNumber} использует Огненный шар по армии противника");
                    }
                    else if (spell is HealSpell)
                    {
                        // Исцеление лечит свою армию
                        targets = _game.Board.GetPlayerArmy(playerNumber);
                        AddLog($"Игрок {playerNumber} использует Исцеление на своей армии");
                    }
                    else if (spell is Booster)
                    {
                        // Усиление применяется к своей армию
                        targets = _game.Board.GetPlayerArmy(playerNumber);
                        AddLog($"Игрок {playerNumber} использует Усиление на своей армии");
                    }

                    // Используем метод Game для розыгрыша заклинания
                    _game.PlayerPlaysSpell(playerNumber, selectedIndex, targets);
                    AddLog($"Игрок {playerNumber} разыграл заклинание");

                    UpdateGameUI();
                    ClearSelections();
                }
                else
                {
                    AddLog("Выбранная карта не является заклинанием!");
                }
            }
            catch (InvalidOperationException ex)
            {
                AddLog($"Не удалось разыграть заклинание: {ex.Message}");
            }
            catch (Exception ex)
            {
                AddLog($"Ошибка: {ex.Message}");
            }
        }

        private void ClearSelections()
        {
            Player1HandList.SelectedIndex = -1;
            Player1CreaturesList.SelectedIndex = -1;
            Player2HandList.SelectedIndex = -1;
            Player2CreaturesList.SelectedIndex = -1;
        }

        private void UpdateGameUI()
        {
            if (_game == null) return;

            try
            {
                // Обновляем индикатор текущего хода
                if (_game.GameStarted)
                {
                    TurnIndicator.Text = $"Ход: Игрок {_game.ActivePlayer.PlayerNumber} (осталось действий: {_game.ActivePlayer.ActionsRemaining})";
                }

                // Информация об игроках
                Player1Info.Text = $"Действий: {_game.Player1.ActionsRemaining}\n" +
                                  $"Карт в руке: {_game.Player1.GetHandCount()}\n" +
                                  $"Мощность руки: {_game.Player1.GetHandPower()}";

                Player2Info.Text = $"Действий: {_game.Player2.ActionsRemaining}\n" +
                                  $"Карт в руке: {_game.Player2.GetHandCount()}\n" +
                                  $"Мощность руки: {_game.Player2.GetHandPower()}";

                // Карты в руке игрока 1
                UpdatePlayerHandList(Player1HandList, _game.Player1);

                // Карты в руке игрока 2
                UpdatePlayerHandList(Player2HandList, _game.Player2);

                // Существа на поле игрока 1
                UpdateCreaturesList(Player1CreaturesList, _game.Board.GetPlayerArmy(1));

                // Существа на поле игрока 2
                UpdateCreaturesList(Player2CreaturesList, _game.Board.GetPlayerArmy(2));

                // Управление активностью кнопок
                UpdateButtonStates();

                // Проверка окончания игры
                if (_game.IsGameOver())
                {
                    var winner = _game.GetWinner();
                    if (winner.HasValue)
                    {
                        TurnIndicator.Text = $"Игра окончена! Победил игрок {winner.Value}";
                        AddLog($"ПОБЕДА! Игрок {winner.Value} победил!");
                        DisableAllActionButtons();
                    }
                    else
                    {
                        TurnIndicator.Text = "Игра окончена ничьей!";
                        AddLog("Игра окончена ничьей!");
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog($"Ошибка обновления UI: {ex.Message}");
            }
        }

        private void UpdateButtonStates()
        {
            if (_game == null) return;

            bool isPlayer1Turn = _game.ActivePlayer.PlayerNumber == 1;
            bool isPlayer2Turn = _game.ActivePlayer.PlayerNumber == 2;

            bool player1HasCards = _game.Player1.GetHandCount() > 0;
            bool player2HasCards = _game.Player2.GetHandCount() > 0;
            bool player1HasCreatures = _game.Board.GetPlayerArmy(1).Count > 0;
            bool player2HasCreatures = _game.Board.GetPlayerArmy(2).Count > 0;
            bool opponent1HasCreatures = _game.Board.GetPlayerArmy(2).Count > 0;
            bool opponent2HasCreatures = _game.Board.GetPlayerArmy(1).Count > 0;

            PlayCardButton1.IsEnabled = isPlayer1Turn && _game.Player1.ActionsRemaining > 0 && player1HasCards;
            AttackButton1.IsEnabled = isPlayer1Turn && _game.Player1.ActionsRemaining > 0 &&
                                      player1HasCreatures && opponent1HasCreatures;
            PlaySpellButton1.IsEnabled = isPlayer1Turn && _game.Player1.ActionsRemaining > 0 && player1HasCards;
            SkipTurnButton1.IsEnabled = isPlayer1Turn && _game.Player1.ActionsRemaining > 0;

            PlayCardButton2.IsEnabled = isPlayer2Turn && _game.Player2.ActionsRemaining > 0 && player2HasCards;
            AttackButton2.IsEnabled = isPlayer2Turn && _game.Player2.ActionsRemaining > 0 &&
                                      player2HasCreatures && opponent2HasCreatures;
            PlaySpellButton2.IsEnabled = isPlayer2Turn && _game.Player2.ActionsRemaining > 0 && player2HasCards;
            SkipTurnButton2.IsEnabled = isPlayer2Turn && _game.Player2.ActionsRemaining > 0;
        }

        private void DisableAllActionButtons()
        {
            PlayCardButton1.IsEnabled = false;
            AttackButton1.IsEnabled = false;
            PlaySpellButton1.IsEnabled = false;
            SkipTurnButton1.IsEnabled = false;
            PlayCardButton2.IsEnabled = false;
            AttackButton2.IsEnabled = false;
            PlaySpellButton2.IsEnabled = false;
            SkipTurnButton2.IsEnabled = false;
        }

        private void UpdatePlayerHandList(ListBox listBox, Player player)
        {
            listBox.Items.Clear();

            for (int i = 0; i < player.GetHandCount(); i++)
            {
                var card = player.PlayerHand.GetCard(i);
                string cardDescription;

                if (card is CreatureCard creatureCard)
                {
                    var creature = creatureCard.CreateCreature();
                    string typeName = creature.GetType().Name;
                    if (typeName == "Assassin") typeName = "Ассасин";
                    else if (typeName == "Knight") typeName = "Рыцарь";
                    else if (typeName == "Wizard") typeName = "Волшебник";

                    cardDescription = $"{typeName} (Урон: {creature.GetDamage()}, Здоровье: {creature.MaxHP}, Стоимость: {card.Power})";
                }
                else if (card is Fireball)
                {
                    cardDescription = $"Огненный шар (Урон: {GameBalanceStats.Fireball.Strenght}, Стоимость: {card.Power})";
                }
                else if (card is HealSpell)
                {
                    cardDescription = $"Исцеление (Лечение: {GameBalanceStats.HealSpell.Strenght}, Стоимость: {card.Power})";
                }
                else if (card is Booster)
                {
                    cardDescription = $"Усиление (+{GameBalanceStats.Booster.Strenght} к урону, Стоимость: {card.Power})";
                }
                else
                {
                    cardDescription = $"{card.GetType().Name} (Стоимость: {card.Power})";
                }

                listBox.Items.Add($"{i + 1}. {cardDescription}");
            }
        }

        private void UpdateCreaturesList(ListBox listBox, List<IBattleable> army)
        {
            listBox.Items.Clear();

            for (int i = 0; i < army.Count; i++)
            {
                if (army[i] is Creature creature)
                {
                    string typeName = creature.GetType().Name;
                    if (typeName == "Assassin") typeName = "Ассасин";
                    else if (typeName == "Knight") typeName = "Рыцарь";
                    else if (typeName == "Wizard") typeName = "Волшебник";

                    int displayHP = creature.HP > 0 ? creature.HP : 0;
                    listBox.Items.Add($"{i + 1}. {typeName}: {displayHP} HP, {creature.GetDamage()} урона");
                }
            }
        }


        private void AutoSave()
        {
            try
            {
                if (_game != null && _game.GameStarted)
                {
                    string autoSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                     "Saves", "autosave.json");

                    // Создаем SaveManager для автосохранения
                    SaveManager saveManager = new SaveManager();
                    saveManager.SaveGame(_game, "autosave");

                    AddLog("Игра автоматически сохранена");
                }
            }
            catch (Exception ex)
            {
                // Не прерываем игру при ошибке автосохранения
                AddLog($"Ошибка автосохранения: {ex.Message}");
            }
        }

        private void EndTurnActions()
        {
            AutoSave();
            UpdateGameUI();
        }

        private void AddLog(string message)
        {
            GameLog.Text = $"{DateTime.Now:HH:mm:ss}: {message}\n" + GameLog.Text;
        }
    }
}