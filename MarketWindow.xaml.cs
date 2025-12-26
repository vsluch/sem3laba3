using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using sem3laba3.Cards;
using sem3laba3.Cards.Creatures;
using sem3laba3.Spells;

namespace sem3laba3
{
    public partial class MarketWindow : Window
    {
        private Game _game;
        private int _currentPlayer; // 1 или 2
        private List<Card> _player1Cards = new List<Card>();
        private List<Card> _player2Cards = new List<Card>();
        private Market _market;

        public MarketWindow()
        {
            InitializeComponent();
            _game = new Game();
            _market = new Market();
            _currentPlayer = 1;

            UpdateUI();
            LoadAvailableCards();
        }

        private void UpdateUI()
        {
            CurrentPlayerText.Text = $"Выбирает: Игрок {_currentPlayer}";
            HandTitle.Text = $"Рука Игрока {_currentPlayer}";

            // Рассчитываем текущую мощность руки
            int currentPower = 0;
            if (_currentPlayer == 1)
                currentPower = CalculateHandPower(_player1Cards);
            else
                currentPower = CalculateHandPower(_player2Cards);

            PowerInfoText.Text = $"Мощность: {currentPower}/100";

            // Обновляем отображение карт в руке
            UpdatePlayerHandDisplay();

            // Активируем/деактивируем кнопки
            NextPlayerButton.IsEnabled = _currentPlayer == 1;
            StartGameButton.IsEnabled = _player1Cards.Count > 0 && _player2Cards.Count > 0;
        }

        private int CalculateHandPower(List<Card> cards)
        {
            int power = 0;
            foreach (var card in cards)
            {
                power += card.Power;
            }
            return power;
        }

        private void LoadAvailableCards()
        {
            AvailableCardsPanel.Children.Clear();

            var availableCards = _market.GetAvailableCards();
            foreach (var card in availableCards)
            {
                AddCardToAvailablePanel(card);
            }
        }

        private void AddCardToAvailablePanel(Card card)
        {
            Border cardBorder = new Border
            {
                BorderBrush = System.Windows.Media.Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, 0, 5),
                Padding = new Thickness(10),
                Background = System.Windows.Media.Brushes.White
            };

            StackPanel cardPanel = new StackPanel();

            // Название карты
            string cardName = GetCardName(card);
            TextBlock nameText = new TextBlock
            {
                Text = cardName,
                FontWeight = FontWeights.Bold,
                FontSize = 14
            };
            cardPanel.Children.Add(nameText);

            // Описание карты
            string description = GetCardDescription(card);
            TextBlock descText = new TextBlock
            {
                Text = description,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 5, 0, 0)
            };
            cardPanel.Children.Add(descText);

            // Мощность карты
            TextBlock powerText = new TextBlock
            {
                Text = $"Мощность: {card.Power}",
                Margin = new Thickness(0, 5, 0, 0)
            };
            cardPanel.Children.Add(powerText);

            // Кнопка добавления
            Button addButton = new Button
            {
                Content = "Добавить в руку",
                Width = 120,
                Margin = new Thickness(0, 5, 0, 0),
                Tag = card
            };
            addButton.Click += AddCardButton_Click;
            cardPanel.Children.Add(addButton);

            cardBorder.Child = cardPanel;
            AvailableCardsPanel.Children.Add(cardBorder);
        }

        private string GetCardName(Card card)
        {
            if (card is CreatureCard creatureCard)
            {
                var creature = creatureCard.CreateCreature();
                return creature.GetType().Name switch
                {
                    "Knight" => "Рыцарь",
                    "Assassin" => "Ассасин",
                    "Wizard" => "Волшебник",
                    _ => creature.GetType().Name
                };
            }
            else if (card is Fireball) return "Огненный шар";
            else if (card is HealSpell) return "Исцеление";
            else if (card is Booster) return "Усиление";
            else return card.GetType().Name;
        }

        private string GetCardDescription(Card card)
        {
            if (card is CreatureCard creatureCard)
            {
                var creature = creatureCard.CreateCreature();
                return $"Существо: {creature.GetDamage()} урона, {creature.MaxHP} здоровья";
            }
            else if (card is Fireball fireball)
            {
                return $"Заклинание: наносит {fireball.Strenght} урона всем существам противника";
            }
            else if (card is HealSpell healSpell)
            {
                return $"Заклинание: лечит {healSpell.Strenght} здоровья всем своим существам";
            }
            else if (card is Booster booster)
            {
                return $"Заклинание: увеличивает урон всех своих существ на {booster.Strenght}";
            }
            else return "Карта";
        }

        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Card card = (Card)button.Tag;

            List<Card> currentPlayerCards = _currentPlayer == 1 ? _player1Cards : _player2Cards;
            int currentPower = CalculateHandPower(currentPlayerCards);

            // Проверяем, можно ли добавить карту
            if (_market.CanAddCardToHand(card, currentPower))
            {
                // Добавляем карту в руку игрока
                currentPlayerCards.Add(card);
                _market.RemoveCard(card);

                // Обновляем интерфейс
                UpdateUI();
                LoadAvailableCards();
            }
            else
            {
                MessageBox.Show($"Нельзя добавить карту! Превышен лимит мощности.\nТекущая мощность: {currentPower}\nМощность карты: {card.Power}\nЛимит: 100",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdatePlayerHandDisplay()
        {
            PlayerHandPanel.Children.Clear();

            List<Card> currentPlayerCards = _currentPlayer == 1 ? _player1Cards : _player2Cards;

            foreach (var card in currentPlayerCards)
            {
                AddCardToHandPanel(card);
            }
        }

        private void AddCardToHandPanel(Card card)
        {
            Border cardBorder = new Border
            {
                BorderBrush = System.Windows.Media.Brushes.LightBlue,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, 0, 5),
                Padding = new Thickness(10),
                Background = System.Windows.Media.Brushes.AliceBlue
            };

            StackPanel cardPanel = new StackPanel();

            // Название карты
            string cardName = GetCardName(card);
            TextBlock nameText = new TextBlock
            {
                Text = cardName,
                FontWeight = FontWeights.Bold
            };
            cardPanel.Children.Add(nameText);

            // Описание карты
            string description = GetCardDescription(card);
            TextBlock descText = new TextBlock
            {
                Text = description,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 5, 0, 0)
            };
            cardPanel.Children.Add(descText);

            // Мощность карты
            TextBlock powerText = new TextBlock
            {
                Text = $"Мощность: {card.Power}",
                Margin = new Thickness(0, 5, 0, 0)
            };
            cardPanel.Children.Add(powerText);

            // Кнопка удаления
            Button removeButton = new Button
            {
                Content = "Убрать из руки",
                Width = 120,
                Margin = new Thickness(0, 5, 0, 0),
                Tag = card,
                Background = System.Windows.Media.Brushes.LightCoral
            };
            removeButton.Click += RemoveCardButton_Click;
            cardPanel.Children.Add(removeButton);

            cardBorder.Child = cardPanel;
            PlayerHandPanel.Children.Add(cardBorder);
        }

        private void RemoveCardButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Card card = (Card)button.Tag;

            List<Card> currentPlayerCards = _currentPlayer == 1 ? _player1Cards : _player2Cards;

            // Удаляем карту из руки игрока
            currentPlayerCards.Remove(card);
            _market.ReturnCard(card);

            // Обновляем интерфейс
            UpdateUI();
            LoadAvailableCards();
        }

        private void NextPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPlayer == 1)
            {
                _currentPlayer = 2;
                NextPlayerButton.IsEnabled = false; // После второго игрока кнопка не нужна
            }

            UpdateUI();

            if (_currentPlayer == 2)
            {
                MessageBox.Show("Теперь выбирает Игрок 2", "Смена игрока",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (_player1Cards.Count == 0 || _player2Cards.Count == 0)
            {
                MessageBox.Show("Оба игрока должны выбрать хотя бы одну карту!",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Создаем игроков с выбранными картами
            Player player1 = new Player(1);
            Player player2 = new Player(2);

            foreach (var card in _player1Cards)
            {
                player1.AddCardToHand(card);
            }

            foreach (var card in _player2Cards)
            {
                player2.AddCardToHand(card);
            }

            // Создаем игру и устанавливаем игроков
            Game game = new Game();

            // Используем Reflection или добавим публичный метод для установки игроков
            // Временно используем рефлексию
            var player1Field = typeof(Game).GetField("_player1", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var player2Field = typeof(Game).GetField("_player2", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (player1Field != null && player2Field != null)
            {
                player1Field.SetValue(game, player1);
                player2Field.SetValue(game, player2);

                // Запускаем игру
                game.StartGame(); // Нужно добавить этот метод

                // Открываем основное окно игры с созданной игрой
                MainWindow mainWindow = new MainWindow(game);
                mainWindow.Show();
                this.Close();
            }
        }
    }
}